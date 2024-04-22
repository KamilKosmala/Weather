using AutoMapper;
using WeatherApi.BusinessLogic.Abstraction.Models;
using WeatherApi.BusinessLogic.Clients.WeatherApi;
using WeatherApi.Data.Entities;

namespace WeatherApi.BusinessLogic.Mappings;

public class WeatherForecastProfile : Profile
{
    public WeatherForecastProfile()
    {
        CreateMap<Location, LocationDto>(); 
        CreateMap<LocationDto, Location>(); 
        
        CreateMap<WeatherForecast, WeatherForecastDto>();
        
        CreateMap<ExternalWeatherModel, List<WeatherForecast>>()
            .ConvertUsing(source => MapWeatherModelToForecast(source));
    }

    private List<WeatherForecast> MapWeatherModelToForecast(ExternalWeatherModel model)
    {
        var forecasts = new List<WeatherForecast>();
        if (model.Hourly == null || model.Hourly.Time == null || model.Hourly.Temperature2M == null) return forecasts;
       
        for (int i = 0; i < model.Hourly.Time.Count; i++)
        {
            forecasts.Add(new WeatherForecast
            {
                Time = DateTime.Parse(model.Hourly.Time[i]),
                TemperatureC = Convert.ToInt32(model.Hourly.Temperature2M[i])
            });
        }
        return forecasts;
    }
}