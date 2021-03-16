using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
//using Microsoft.Identity.Web.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IFeatureManager _featureManager;

        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "access_as_user" };

        public WeatherForecastController(IConfiguration configuration, ILogger<WeatherForecastController> logger, IFeatureManager featureManager)
        {
            _logger = logger;
            _configuration = configuration;
            _featureManager = featureManager;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            // TODO
            // HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

            if (_featureManager.IsEnabledAsync(nameof(FeatureFlags.KillSwitch)).Result)
            {
                _logger.LogCritical("{Message} @ {Time}", "KILL SWITCH ENABLED", DateTime.UtcNow);
                throw new NotFiniteNumberException("KILL SWITCH ENABLED");
            }

            int topN = Int32.Parse(_configuration["number"]);

            var rng = new Random();
            return Enumerable.Range(1, topN).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
