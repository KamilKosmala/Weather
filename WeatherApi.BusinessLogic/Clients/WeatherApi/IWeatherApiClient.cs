namespace WeatherApi.BusinessLogic.Clients.WeatherApi;

public interface IWeatherApiClient
{ 
    Task<ExternalWeatherModel?> FetchWeatherAsync(double latitude, double longitude);
}