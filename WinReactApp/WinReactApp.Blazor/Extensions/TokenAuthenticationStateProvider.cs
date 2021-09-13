namespace WinReactApp.Blazor.Extensions
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.JSInterop;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using WinReactApp.ResourceModel.UserAuth;

    public class TokenAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly NavigationManager _navigationManager;

        public TokenAuthenticationStateProvider(IJSRuntime jsRuntime, NavigationManager navigationManager)
        {
            _jsRuntime = jsRuntime;
            _navigationManager = navigationManager;
        }

        public async Task SetTokenAsync(string token, AuthTokenResourceModel authTokenRM = null, DateTime expiry = default)
        {
            if (token == null)
            {
                await _jsRuntime.InvokeAsync<object>("localStorage.removeItem", "authToken");
                await _jsRuntime.InvokeAsync<object>("localStorage.removeItem", "authTokenExpiry");
            }
            else
            {
                await _jsRuntime.InvokeAsync<object>("localStorage.setItem", "authToken", token);
                await _jsRuntime.InvokeAsync<object>("localStorage.setItem", "authTokenExpiry", expiry);
            }

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<string> GetTokenAsync()
        {
            var expiry = await _jsRuntime.InvokeAsync<object>("localStorage.getItem", "authTokenExpiry");
            if (expiry != null)
            {
                if (DateTime.Parse(expiry.ToString()) > DateTime.Now)
                {
                    return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
                }
                else
                {
                    await SetTokenAsync(null);
                }
            }
            return null;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await GetTokenAsync();
            var identity = string.IsNullOrEmpty(token)
                ? new ClaimsIdentity()
                : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        public async Task ValidateUserAuthenticationForPageAsync()
        {
            var authenticationState = await this.GetAuthenticationStateAsync();

            if (authenticationState?.User?.Identity is null || !authenticationState.User.Identity.IsAuthenticated)
            {
                _navigationManager.NavigateTo("login", true);
            }
        }

        public async Task<UserDetailsResourceModel> GetUserClaimsAsync()
        {
            var authenticationState = await this.GetAuthenticationStateAsync();

            UserDetailsResourceModel userDetailsRM = new UserDetailsResourceModel();

            foreach (var item in authenticationState.User.Claims)
            {
                if (item.Type.ToLower().Contains("nameidentifier"))
                {
                    userDetailsRM.UserId = item.Value;
                }

                if (item.Type.ToLower().Contains("name"))
                {
                    userDetailsRM.UserName = item.Value;
                }

                if (item.Type.ToLower().Contains("emailaddress"))
                {
                    userDetailsRM.EmailAddress = item.Value;
                }

                if (item.Type.ToLower().Contains("expiration"))
                {
                    userDetailsRM.Expiration = item.Value;
                }

                if (item.Type.ToLower().Contains("jti"))
                {
                    userDetailsRM.Jti = item.Value;
                }

                if (item.Type.ToLower().Contains("role"))
                {
                    var roles = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(item.Value);

                    foreach (var role in roles)
                    {
                        if (!string.IsNullOrEmpty(role) && role != "null")
                        {
                            string role1 = role.Replace("'", "");

                            userDetailsRM.Roles.Add(role1);
                        }
                    }
                }
            }

            return userDetailsRM;
        }

        public async Task ValidateRequestAsync(HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                _navigationManager.NavigateTo("login");
            }
            else if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
            {
                await this._jsRuntime.InvokeVoidAsync("sharedController.handleForbiddenHttpError");
            }
            else if (httpResponseMessage.StatusCode == HttpStatusCode.InternalServerError)
            {
                //var requestId = httpResponseMessage.Headers.FirstOrDefault(x => x.Key == "X-Request-Id").Value.FirstOrDefault();

                string requestId = string.Empty;

                foreach (var item in httpResponseMessage.Headers)
                {
                    if (item.Key.ToLower() == "x-request-id")
                    {
                        requestId = item.Value.FirstOrDefault();
                    }
                }

                //var stBuilder = new StringBuilder();
                //foreach (var header in httpResponseMessage.Headers)
                //    stBuilder.AppendLine($"'{header.Key}': [{String.Join(',', header.Value)}]");
                //foreach (var header in httpResponseMessage.TrailingHeaders)
                //    stBuilder.AppendLine($"'{header.Key}': [{String.Join(',', header.Value)}]");
                //stBuilder.AppendLine("End of headers! Total header count: " + (httpResponseMessage.Headers.Count() + httpResponseMessage.TrailingHeaders.Count()) + " StatusCode: " + httpResponseMessage.StatusCode);

                //var vvvvv = stBuilder.ToString();

                await this._jsRuntime.InvokeVoidAsync("sharedController.handleHttpInternalServerError", requestId);
            }
        }
    }
}