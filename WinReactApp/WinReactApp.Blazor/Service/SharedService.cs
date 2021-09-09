namespace WinReactApp.Blazor.Service
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class SharedService
    {
        private readonly HttpClient _httpClient;

        //private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navigationManager;

        private readonly IJSRuntime _jsRuntime;

        public SharedService(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        [JSInvokable]
        public async Task NavigateToPageAsync(string page)
        {
            _navigationManager.NavigateTo(page);
        }
    }
}