namespace WinReactApp.Blazor.Clients
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using WinReactApp.Blazor.Extensions;

    public class AuthValidationClient
    {
        private HttpClient _httpClient;

        private TokenAuthenticationStateProvider _authenticationStateProvider;

        public AuthValidationClient(HttpClient httpClient, TokenAuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<HttpResponseMessage> ValidateAuthenticationAsync()
        {
            var token = await _authenticationStateProvider.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var response = await _httpClient.GetAsync("UserAuth/ValidateAuthentication?api-version=1.0");

            await _authenticationStateProvider.ValidateRequestAsync(response);

            return response;
        }

        public async Task<HttpResponseMessage> ValidateAdministrator1RoleAsync()
        {
            var token = await _authenticationStateProvider.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            _httpClient.DefaultRequestHeaders.Add("Accept", "*/*");

            var response = await _httpClient.GetAsync("UserAuth/IsUserAdmin1?api-version=1.0");

            await _authenticationStateProvider.ValidateRequestAsync(response);

            return response;
        }

        public async Task<HttpResponseMessage> RaiseError()
        {
            var token = await _authenticationStateProvider.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var response = await _httpClient.GetAsync("UserAuth/RaiseError?api-version=1.0");

            await _authenticationStateProvider.ValidateRequestAsync(response);

            return response;
        }
    }
}