namespace WeatherApi.Data.Entities;

public class WeatherForecast: BaseEntity
{
 
    public DateTime Time { get; set; }
    public int TemperatureC { get; set; }
    
    public int LocationId { get; set; }
    public virtual Location? Location { get; set; } 
}