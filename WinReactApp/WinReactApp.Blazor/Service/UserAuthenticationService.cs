namespace WinReactApp.Blazor.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class UserAuthenticationService
    {
        private readonly HttpClient httpClient;

        public UserAuthenticationService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
    }
}