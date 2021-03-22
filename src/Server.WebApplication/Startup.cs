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
                _logger.LogWarning($"DEV ENVIRONMENT");

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
                    _logger.LogInformation($"post={DateTime.Now}");

                    if (_featureManager.IsEnabledAsync(nameof(FeatureFlags.EnableKillSwitch)).Result)
                    {
                        string message = $"Time:{DateTime.UtcNow},EnableKillSwitch={FeatureFlags.EnableKillSwitch}";
                        _logger.LogCritical(message);
                        throw new NotImplementedException(message);
                    }

                    context.Response.Headers.Add("post", DateTime.UtcNow.ToString());

                    await context.Response.WriteAsync("");
                });
            });
        }
    }
}
