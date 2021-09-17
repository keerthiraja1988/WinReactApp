namespace WinReactApp.Blazor.Clients
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using WinReactApp.Blazor.Extensions;
    using WinReactApp.Blazor.Pages.Authentication;
    using WinReactApp.ResourceModel.UserAuth;

    public class ManageUserClient
    {
        private HttpClient _httpClient;

        private TokenAuthenticationStateProvider _authenticationStateProvider;

        public ManageUserClient(HttpClient httpClient, TokenAuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<HttpResponseMessage> GetAllAddressesAsync()
        {
            var token = await _authenticationStateProvider.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var response = await _httpClient.GetAsync("Address/GetAllAddresses?api-version=1.0");

            await _authenticationStateProvider.ValidateRequestAsync(response);

            return response;
        }
    }
}