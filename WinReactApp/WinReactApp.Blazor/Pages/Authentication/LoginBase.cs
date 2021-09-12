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
                this._sharedServiceObjRef = DotNetObjectReference.Create(_sharedService);
                await this._jsRuntime.InvokeVoidAsync("authController.onSuccessUserLogin", _sharedServiceObjRef);
            }

            await this._jsRuntime.InvokeVoidAsync("sharedController.hideLoadingIndicator");
        }

        public async Task ClearRegisterForm()
        {
            await this._jsRuntime.InvokeVoidAsync("sharedController.clearValidationSummary");

            LoginUserRM = new LoginUserResourseModel();
        }

        [JSInvokable]
        public void NavigateToPage(string page)
        {
            _navigationManager.NavigateTo(page);
        }
    }
}