using AutoMapper;
using WeatherApi.BusinessLogic.Abstraction.Models;
using WeatherApi.Data.Entities;
using WeatherApi.Models;

namespace WeatherApi.Mappings;

public class WeatherForecastProfile : Profile
{
    public WeatherForecastProfile()
    {
        CreateMap<LocationRequest, LocationDto>();
        
        CreateMap<LocationDto, LocationResponse>();
        CreateMap<LocationDto, LocationListResponse>();
        
        CreateMap<WeatherForecastDto, WeatherForecastResponse>();
    }
}