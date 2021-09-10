namespace WinReactApp.Blazor
{
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

            await builder.Build().RunAsync();
        }

        public static async Task ConfigureServices(WebAssemblyHostBuilder builder, HttpClient http)
        {
            using var response = await http.GetAsync("appsettings.json");
            using var stream = await response.Content.ReadAsStreamAsync();

            builder.Configuration.AddJsonStream(stream);
        }
    }
}