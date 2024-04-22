namespace WeatherApi.BusinessLogic.Abstraction.Models;

public class LocationDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public List<WeatherForecastDto> WeatherForecasts { get; set; } = new();
}