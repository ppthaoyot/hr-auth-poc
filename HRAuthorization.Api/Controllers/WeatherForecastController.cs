using HRAuthorization.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HRAuthorization.Api.Controllers;

[ApiController]
[Route("weatherforecast")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherService _service;

    public WeatherForecastController(IWeatherService service)
        => _service = service;

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
        => _service.GetForecast();
}
