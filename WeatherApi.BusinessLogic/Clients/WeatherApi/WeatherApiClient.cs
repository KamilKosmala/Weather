using System.Globalization;
using Newtonsoft.Json;

namespace WeatherApi.BusinessLogic.Clients.WeatherApi;

public class WeatherApiClient : IWeatherApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://api.open-meteo.com/v1/forecast";
    private readonly CultureInfo _customCulture;

    public WeatherApiClient()
    {
        _httpClient = new HttpClient();
        _customCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        _customCulture.NumberFormat.NumberDecimalSeparator = ".";
    }

    public async Task<ExternalWeatherModel?> FetchWeatherAsync(double latitude, double longitude)
    {
        var latitudeValue = latitude.ToString(_customCulture);
        var longitudeValue = longitude.ToString(_customCulture);
        var url = $"{_baseUrl}?latitude={latitudeValue}&longitude={longitudeValue}&hourly=temperature_2m";
        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ExternalWeatherModel>(content);
        }
        throw new HttpRequestException("Failed to retrieve weather data");
    }
}