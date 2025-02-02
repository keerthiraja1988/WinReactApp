﻿using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WinReactApp.UserAuth
{
	public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
	{
		private readonly IApiVersionDescriptionProvider _provider;

		public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

		public void Configure(SwaggerGenOptions options)
		{
			// add a swagger document for each discovered API version
			// note: you might choose to skip or document deprecated API versions differently
			foreach (var description in _provider.ApiVersionDescriptions)
			{
				options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
			}
		}

		private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
		{
			var info = new OpenApiInfo()
			{
				Title = "WinReactApp User Auth",
				Version = description.ApiVersion.ToString(),
			};

			if (description.IsDeprecated)
			{
				info.Description += " This API version has been deprecated.";
			}

			return info;
		}
	}
}