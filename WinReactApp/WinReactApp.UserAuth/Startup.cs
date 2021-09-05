namespace WinReactApp.UserAuth
{
    using AutoMapper;
    using FluentValidation.AspNetCore;
    using Insight.Database;
    using MicroElements.Swashbuckle.FluentValidation;
    using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System.Data.SqlClient;
    using System.Reflection;
    using WinReactApp.UserAuth.Extensions.AutoMapper;
    using WinReactApp.UserAuth.Extensions.Swagger;
    using WinReactApp.UserAuth.Repository;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 1);
                options.ReportApiVersions = true;
                options.ApiVersionReader =
                         ApiVersionReader.Combine(
                            new HeaderApiVersionReader("X-Api-Version"),
                            new QueryStringApiVersionReader("api-version"));
            });

            services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });

            string sqlConnectionString = "Data Source=.;Initial Catalog=WinReactApp.UserAuth;Integrated Security=True;Persist Security Info=true;";

            services.AddHttpContextAccessor();
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());

            var _sqlConnection = new SqlConnection(sqlConnectionString);
            services.AddTransient(b => _sqlConnection.AsParallel<IUserAuthenticationRepository>());

            services.AddControllers()
                  .AddJsonOptions(options =>
                  {
                      options.JsonSerializerOptions.PropertyNamingPolicy = null;
                  }).AddFluentValidation(options =>
                  {
                      //options.RegisterValidatorsFromAssemblyContaining<Startup>();
                      options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                      options.ValidatorFactoryType = typeof(HttpContextServiceProviderValidatorFactory);
                  });

            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddFluentValidationRulesToSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            app.Use(async (context, next) =>
            {
                string apiVersion = context.Request.Query["api-version"];

                if (!string.IsNullOrEmpty(apiVersion))
                {
                    context.Response.Headers.Add("X-Api-Version", apiVersion);
                }
                else
                {
                    context.Response.Headers.Add("X-Api-Version", "1.1");
                }

                context.Response.Headers.Add("X-Default-Api-Version", "1.1");
                context.Response.Headers.Add("X-Request-Id", context.TraceIdentifier);
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");

                // Call the next delegate/middleware in the pipeline
                await next();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                app.UseSwaggerUI(
                options =>
                {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}