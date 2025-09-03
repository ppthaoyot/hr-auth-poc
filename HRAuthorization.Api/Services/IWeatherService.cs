namespace HRAuthorization.Api.Services;

public interface IWeatherService
{
    IEnumerable<WeatherForecast> GetForecast();
}
