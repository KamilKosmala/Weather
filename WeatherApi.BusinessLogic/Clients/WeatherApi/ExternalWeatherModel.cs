using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace WeatherApi.BusinessLogic.Clients.WeatherApi;

public class ExternalWeatherModel
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double GenerationTimeMs { get; set; }
    public int UtcOffsetSeconds { get; set; }
    public string? Timezone { get; set; }
    public string? TimezoneAbbreviation { get; set; }
    public double Elevation { get; set; }
    [JsonProperty("hourly_units")]
    public ExternalHourlyUnits? HourlyUnits { get; set; }
    public ExternalHourlyData? Hourly { get; set; }
    
    public class ExternalHourlyUnits
    {
        public string? Time { get; set; }
        [JsonProperty("temperature_2m")]
        public string? Temperature2M { get; set; }
    }

    public class ExternalHourlyData
    {
        public List<string> Time { get; set; } = new();
        [JsonProperty("temperature_2m")]
        public List<double> Temperature2M { get; set; } = new();
    }
}