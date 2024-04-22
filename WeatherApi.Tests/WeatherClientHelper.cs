using WeatherApi.BusinessLogic.Clients.WeatherApi;

namespace WeatherApi.Tests;

public static class WeatherClientHelper
{
    public static ExternalWeatherModel GetSampleData()
    {
        return new ExternalWeatherModel
        {
            Latitude = 20.0,
            Longitude = 40.0,
            Hourly = new ExternalWeatherModel.ExternalHourlyData
            {
                Time =
                {
                    "2024-04-25T04:00", "2024-04-25T05:00", "2024-04-25T06:00"
                },
                Temperature2M = [1d, 2d, 3d]
            }
        };
    }
}