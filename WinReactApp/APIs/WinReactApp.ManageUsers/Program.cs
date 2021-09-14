//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <author>Keerthi</author>
//-----------------------------------------------------------------------
namespace WinReactApp.ManageUsers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json.Linq;
    using NLog;
    using NLog.Extensions.Logging;
    using NLog.Web;

    public class Program
    {
#pragma warning disable SA1306 // Field names should begin with lower-case letter
#pragma warning disable SA1310 // Field names should not contain underscore
        private static string ASPNETCORE_ENVIRONMENT;
#pragma warning restore SA1310 // Field names should not contain underscore
#pragma warning restore SA1306 // Field names should begin with lower-case letter

        public static void Main(string[] args)
        {
            var envSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "enviromentSettings.json");
            var envSettings = JObject.Parse(File.ReadAllText(envSettingsPath));
            ASPNETCORE_ENVIRONMENT = envSettings["ASPNETCORE_ENVIRONMENT"].ToString();

            var config = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
#if DEBUG

                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
#else
               .AddJsonFile($"appsettings.{ASPNETCORE_ENVIRONMENT}.json", optional: true, reloadOnChange: true)
#endif
               .Build();

            LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));

            var logger = NLog.Web.NLogBuilder.ConfigureNLog(LogManager.Configuration).GetCurrentClassLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>

            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            })
           .UseNLog();  // NLog: Setup NLog for Dependency injection;
    }
}