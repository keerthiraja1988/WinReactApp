namespace WinReactApp.Blazor.Clients
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using WinReactApp.Blazor.Pages.Authentication;
    using WinReactApp.ResourceModel.UserAuth;

    public class UserAuthenticationClient
    {
        private HttpClient _httpClient;

        public UserAuthenticationClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> RegisterUserAsync(StringContent stringContent)
        {
            var response = await _httpClient.PostAsync("UserAuth/registeruser?api-version=1.0", stringContent);

            return response;
        }

        public async Task<HttpResponseMessage> LoginUserAsync(StringContent stringContent)
        {
            var response = await _httpClient.PostAsync("UserAuth/LoginUser?api-version=1.0", stringContent);

            return response;
        }
    }
}