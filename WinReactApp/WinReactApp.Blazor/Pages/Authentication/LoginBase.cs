namespace WinReactApp.Blazor.Pages.Authentication
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using WinReactApp.Blazor.Clients;
    using WinReactApp.Blazor.Extensions;
    using WinReactApp.Blazor.Extensions.Authentication;
    using WinReactApp.Blazor.Service;
    using WinReactApp.ResourceModel.UserAuth;

    public class LoginBase : ComponentBase
    {
        [Inject]
        protected IJSRuntime _jsRuntime { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        [Inject]
        protected SharedService _sharedService { get; set; }

        [Inject]
        protected UserAuthenticationClient _userAuthenticationClient { get; set; }

        [Inject]
        protected TokenAuthenticationStateProvider _authStateProvider { get; set; }

        protected DotNetObjectReference<SharedService> _sharedServiceObjRef;
        protected LoginUserResourseModel LoginUserRM = new LoginUserResourseModel();
        protected ServerValidator serverValidator;

        public async Task SubmitRegisterForm()
        {
            await this._jsRuntime.InvokeVoidAsync("sharedController.showLoadingIndicator");

            await this._jsRuntime.InvokeVoidAsync("sharedController.clearValidationSummary");

            StringContent content = new StringContent(JsonConvert.SerializeObject(LoginUserRM), Encoding.UTF8, "application/json");

            var response = await this._userAuthenticationClient.LoginUserAsync(content);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                serverValidator.Validate(response, content);
            }
            else
            {
                var body = await response.Content.ReadAsStringAsync();
                AuthTokenResourceModel authTokenResourceModel = System.Text.Json.JsonSerializer.Deserialize<AuthTokenResourceModel>(body);

                Console.WriteLine(authTokenResourceModel.AuthToken);

                await _authStateProvider.SetTokenAsync(authTokenResourceModel.AuthToken, authTokenResourceModel.ExpireOn);

                this._sharedServiceObjRef = DotNetObjectReference.Create(_sharedService);
                _navigationManager.NavigateTo("");
            }

            await this._jsRuntime.InvokeVoidAsync("sharedController.hideLoadingIndicator");
        }

        public async Task ClearRegisterForm()
        {
            await this._jsRuntime.InvokeVoidAsync("sharedController.clearValidationSummary");

            LoginUserRM = new LoginUserResourseModel();
        }
    }
}