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
    using WinReactApp.Blazor.Service;
    using WinReactApp.ResourceModel.UserAuth;

    public class RegisterUserBase : ComponentBase
    {
        [Inject]
        protected IJSRuntime _jsRuntime { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        [Inject]
        protected SharedService _sharedService { get; set; }

        [Inject]
        protected UserAuthenticationClient _userAuthenticationClient { get; set; }

        protected DotNetObjectReference<SharedService> _sharedServiceObjRef;
        protected RegisterUserResourseModel RegisterUserRM = new RegisterUserResourseModel();
        protected ServerValidator serverValidator;

        public async Task HandleValidSubmit()
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(RegisterUserRM), Encoding.UTF8, "application/json");

            var response = await this._userAuthenticationClient.RegisterUserAsync(content);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                serverValidator.Validate(response, content);
            }
            else
            {
                this._sharedServiceObjRef = DotNetObjectReference.Create(_sharedService);
                await this._jsRuntime.InvokeVoidAsync("authController.onSuccessRegistration", _sharedServiceObjRef);
            }
        }

        public async Task ClearRegisterForm()
        {
            await this._jsRuntime.InvokeVoidAsync("sharedController.clearValidationSummary");

            RegisterUserRM = new RegisterUserResourseModel();
        }

        [JSInvokable]
        public void NavigateToPage(string page)
        {
            _navigationManager.NavigateTo(page);
        }
    }
}