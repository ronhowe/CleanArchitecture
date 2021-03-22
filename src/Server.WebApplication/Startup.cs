using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;
using System;

namespace Server.WebApplication
{
    public class Startup
    {
        private IConfiguration _configuration;
        private ILogger<Startup> _logger;
        private IFeatureManager _featureManager;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                // TODO - Implement AddMicrosoftIdentityWebApi
                //.AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));
                .AddJwtBearer(opt =>
                    {
                        opt.Audience = _configuration["AAD:ResourceId"];
                        opt.Authority = $"{_configuration["AAD:Instance"]}{_configuration["AAD:TenantId"]}";
                    });

            services.AddAzureAppConfiguration();

            services.AddHealthChecks();

            services.AddControllers();

            services.AddFeatureManagement();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Server.WebApplication", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger, IFeatureManager featureManager)
        {
            _logger = logger;
            _featureManager = featureManager;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Server.WebApplication v1"));
            }

            app.UseAzureAppConfiguration();

            app.UseHealthChecks("/health");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapGet("/", async context =>
                {
                    MockLogMessage();

                    MockApplicationException();

                    AddPostHeader(context);

                    AddDevelopmentHeader(env, context);

                    AddHostHeader(context);

                    await context.Response.WriteAsync(String.Empty);
                });
            });

            void MockLogMessage()
            {
                if
                (
                    _featureManager.IsEnabledAsync(nameof(FeatureFlags.MockLogMessages)).Result &&
                    _featureManager.IsEnabledAsync(nameof(FeatureFlags.EnableLogDebug)).Result
                )
                {
                    _logger.LogDebug($"{DateTime.UtcNow} FeatureFlags.MockLogMessages");
                }

                if
                (
                    _featureManager.IsEnabledAsync(nameof(FeatureFlags.MockLogMessages)).Result &&
                    _featureManager.IsEnabledAsync(nameof(FeatureFlags.EnableLogTrace)).Result
                )
                {
                    _logger.LogTrace($"{DateTime.UtcNow} FeatureFlags.MockLogMessages");
                }

                if
                (
                    _featureManager.IsEnabledAsync(nameof(FeatureFlags.MockLogMessages)).Result &&
                    _featureManager.IsEnabledAsync(nameof(FeatureFlags.EnableLogInformation)).Result
                )
                {
                    _logger.LogInformation($"{DateTime.UtcNow} FeatureFlags.MockLogMessages");
                }

                if
                (
                    _featureManager.IsEnabledAsync(nameof(FeatureFlags.MockLogMessages)).Result &&
                    _featureManager.IsEnabledAsync(nameof(FeatureFlags.EnableLogWarning)).Result
                )
                {
                    _logger.LogWarning($"{DateTime.UtcNow} FeatureFlags.MockLogMessages");
                }

                if
                (
                    _featureManager.IsEnabledAsync(nameof(FeatureFlags.MockLogMessages)).Result &&
                    _featureManager.IsEnabledAsync(nameof(FeatureFlags.EnableLogError)).Result
                )
                {
                    _logger.LogError($"{DateTime.UtcNow} FeatureFlags.MockLogMessages");
                }

                if
                (
                    _featureManager.IsEnabledAsync(nameof(FeatureFlags.MockLogMessages)).Result &&
                    _featureManager.IsEnabledAsync(nameof(FeatureFlags.EnableLogCritical)).Result
                )
                {
                    _logger.LogCritical($"{DateTime.UtcNow} FeatureFlags.MockLogMessages");
                }
            }

            void MockApplicationException()
            {
                if (_featureManager.IsEnabledAsync(nameof(FeatureFlags.MockApplicationException)).Result)
                {
                    throw new NotImplementedException($"{DateTime.UtcNow} FeatureFlags.MockApplicationException");
                }
            }

            void AddPostHeader(HttpContext context)
            {
                if (_featureManager.IsEnabledAsync(nameof(FeatureFlags.EnableLogTrace)).Result)
                {
                    _logger.LogTrace("Adding x-application-post header.");
                }

                context.Response.Headers.Add("x-application-post", $"post={DateTime.UtcNow}");
            }

            void AddDevelopmentHeader(IWebHostEnvironment env, HttpContext context)
            {
                if (env.IsDevelopment())
                {
                    if (_featureManager.IsEnabledAsync(nameof(FeatureFlags.EnableLogTrace)).Result)
                    {
                        _logger.LogTrace("Adding x-application-evelopment header.");
                    }

                    context.Response.Headers.Add("x-application-development", "development");
                }
            }

            void AddHostHeader(HttpContext context)
            {
                if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME")))
                {
                    if (_featureManager.IsEnabledAsync(nameof(FeatureFlags.EnableLogTrace)).Result)
                    {
                        _logger.LogTrace("Adding x-application-host header.");
                    }

                    context.Response.Headers.Add("x-application-host", Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"));
                }
            }
        }
    }
}
