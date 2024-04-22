using WeatherApi.BusinessLogic.Abstraction.Models;

namespace WeatherApi.BusinessLogic.Abstraction.Interfaces;

public interface IWeatherService
{
    Task<LocationDto?> GetForecastAsync(int locationId);

    Task<PagedResult<LocationDto>> GetAllLocations(int pageNumber, int pageSize);
    Task DeleteLocation(int locationId);

    Task AddLocation(LocationDto locationDto);
}