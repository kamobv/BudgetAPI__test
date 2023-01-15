using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        //Auth
        //{baseUrl}/Auth/Login
        //{baseUrl}/Auth/Register
        //{baseUrl}/Auth/Update
        //{baseUrl}/Auth/ChangePassword

        //User
        //{baseUrl}/User/List
        //{baseUrl}/User/List/UserId
        //{baseUrl}/User/Update
        //{baseUrl}/User/Delete

        //Transaction
        //{baseUrl}/Transaction/List/UserId
        //{baseUrl}/Transaction/List/UserId/DateRange,TypeType
        //Create,Update,Delete


        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
