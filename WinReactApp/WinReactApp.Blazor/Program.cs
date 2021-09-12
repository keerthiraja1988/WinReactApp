namespace WinReactApp.Blazor
{
    using Blazored.LocalStorage;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using WinReactApp.Blazor.Clients;
    using WinReactApp.Blazor.Extensions.Authentication;
    using WinReactApp.Blazor.Service;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddSingleton<SharedService>();

            var http = new HttpClient()
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            };

            builder.Services.AddScoped(sp => http);

            await ConfigureServices(builder, http);

            builder.Services.AddHttpClient<UserAuthenticationClient>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["API_URLS:WinReactApp.UserAuth"]);
            });

            builder.Services.AddScoped<TokenAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<TokenAuthenticationStateProvider>());

            builder.Services.AddBlazoredLocalStorage();

            await builder.Build().RunAsync();
        }

        public static async Task ConfigureServices(WebAssemblyHostBuilder builder, HttpClient http)
        {
            using var enviromentResponse = await http.GetAsync("enviromentSettings.json");
            string enviromentStream = await enviromentResponse.Content.ReadAsStringAsync();
            var envSettings = JObject.Parse(enviromentStream);

            var ASPNETCORE_ENVIRONMENT = envSettings["ASPNETCORE_ENVIRONMENT"].ToString();

            string appSettingsFile = string.Empty;

            if (ASPNETCORE_ENVIRONMENT == "Debug")
            {
                appSettingsFile = "appsettings.json";
            }
            else
            {
                appSettingsFile = "appsettings." + ASPNETCORE_ENVIRONMENT + ".json";
            }

            using var response = await http.GetAsync(appSettingsFile);
            using var stream = await response.Content.ReadAsStreamAsync();

            builder.Configuration.AddJsonStream(stream);
        }
    }
}