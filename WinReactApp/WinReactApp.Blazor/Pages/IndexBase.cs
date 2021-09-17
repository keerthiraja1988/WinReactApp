namespace WinReactApp.Blazor.Pages
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WinReactApp.Blazor.Clients;
    using WinReactApp.Blazor.Extensions;
    using WinReactApp.Blazor.Service;
    using WinReactApp.ResourceModel.ManageUsers;

    public class IndexBase : ComponentBase
    {
        [Inject]
        protected IJSRuntime _jsRuntime { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        [Inject]
        protected SharedService _sharedService { get; set; }

        [Inject]
        protected ManageUserClient _manageUserClient { get; set; }

        [Inject]
        protected TokenAuthenticationStateProvider _authenticationStateProvider { get; set; }

        protected List<GetAddressResourseModel> UserAddresses = new List<GetAddressResourseModel>();

        public async Task GetAllAddressesAsync()
        {
            var response = await this._manageUserClient.GetAllAddressesAsync();

            var body = await response.Content.ReadAsStringAsync();

            UserAddresses = System.Text.Json.JsonSerializer.Deserialize<List<GetAddressResourseModel>>(body);

            await this._jsRuntime.InvokeVoidAsync("sharedController.hideLoadingIndicator");
        }
    }
}