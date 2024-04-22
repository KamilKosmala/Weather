namespace WeatherApi.Models;

public class LocationResponse
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public List<WeatherForecastResponse> WeatherForecasts { get; set; } = new();
}