namespace WeatherApi.Data.Entities;

public class Location: BaseEntity
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public virtual List<WeatherForecast> WeatherForecasts { get; set; } = new();
}