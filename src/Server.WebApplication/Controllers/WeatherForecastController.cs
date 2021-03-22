using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Server.WebApplication.Models;
//using Microsoft.Identity.Web.Resource;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.WebApplication.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IConfiguration _configuration;
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IFeatureManager _featureManager;

        // TODO - Implement scopeRequiredByApi
        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "access_as_user" };

        public WeatherForecastController(IConfiguration configuration, ILogger<WeatherForecastController> logger, IFeatureManager featureManager)
        {
            _configuration = configuration;
            _logger = logger;
            _featureManager = featureManager;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            // TODO - Implement VerifyUserHasAnyAcceptedScope
            // HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

            if (_featureManager.IsEnabledAsync(nameof(FeatureFlags.MockApplicationException)).Result)
            {
                string message = $"Time:{DateTime.UtcNow},MockApplicationException={FeatureFlags.MockApplicationException}";
                _logger.LogCritical(message);
                throw new NotImplementedException(message);
            }

            var rng = new Random();
            return Enumerable.Range(1, 3).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
