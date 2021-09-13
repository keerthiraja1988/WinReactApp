namespace WinReactApp.UserAuth
{
    using System.Data.SqlClient;
    using System.Reflection;
    using System.Text;
    using AutoMapper;
    using FluentValidation.AspNetCore;
    using Insight.Database;
    using MicroElements.Swashbuckle.FluentValidation;
    using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using NLog.Extensions.Logging;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using WinReactApp.UserAuth.Extensions.AutoMapper;
    using WinReactApp.UserAuth.Extensions.Filters;
    using WinReactApp.UserAuth.Extensions.Swagger;
    using WinReactApp.UserAuth.Repository;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
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

            string sqlConnectionString = this.Configuration["DatabaseSetting:SqlDbConnection"];

            services.AddHttpContextAccessor();
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());

            var sqlConnection = new SqlConnection(sqlConnectionString);
            services.AddTransient(b => sqlConnection.AsParallel<IUserAuthenticationRepository>());

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(LoggingActionFilter));
            })
                  .AddJsonOptions(options =>
                  {
                      options.JsonSerializerOptions.PropertyNamingPolicy = null;
                  }).AddFluentValidation(options =>
                  {
                      options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                      options.ValidatorFactoryType = typeof(HttpContextServiceProviderValidatorFactory);
                  });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

          // Adding Jwt Bearer
          .AddJwtBearer(options =>
          {
              options.SaveToken = true;
              options.RequireHttpsMetadata = true;
              options.TokenValidationParameters = new TokenValidationParameters()
              {
                  ValidateIssuer = true,
                  ValidateAudience = true,
                  ValidAudience = this.Configuration["JWT:ValidAudience"],
                  ValidIssuer = this.Configuration["JWT:ValidIssuer"],
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["JWT:Secret"])),
              };
          });

            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddFluentValidationRulesToSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
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

            app.UseCors(builder => builder
                     .AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     .WithExposedHeaders("X-Request-Id"));

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseExceptionHandler(a => a.Run(async context =>
            {
                context.Response.Headers.Add("X-Request-Id", context.TraceIdentifier);

                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;

                if (env.IsDevelopment())
                {
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Error = "An error occured while processing your request.",
                        IsError = true,
                        StatusCode = 500,
                        StackTrace = exception.StackTrace,
                        Message = exception.Message,
                        InnerException = exception.InnerException,
                    });
                }
                else
                {
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Error = "An error occured while processing your request.",
                        IsError = true,
                        StatusCode = 500,
                    });
                }
            }));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}