namespace WinReactApp.Blazor.Pages
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WinReactApp.Blazor.Extensions.Authentication;
    using WinReactApp.Blazor.Service;

    public class IndexBase : ComponentBase
    {
        [Inject]
        protected IJSRuntime _jsRuntime { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        [Inject]
        protected SharedService _sharedService { get; set; }

        [Inject]
        protected TokenAuthenticationStateProvider _authenticationStateProvider { get; set; }
    }
}