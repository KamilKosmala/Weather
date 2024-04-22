using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WeatherApi.BusinessLogic.Abstraction.Interfaces;
using WeatherApi.BusinessLogic.Abstraction.Models;
using WeatherApi.BusinessLogic.Clients;
using WeatherApi.BusinessLogic.Clients.WeatherApi;
using WeatherApi.Data;
using WeatherApi.Data.Entities;

namespace WeatherApi.BusinessLogic.Services;

public class WeatherService : IWeatherService
{
    private readonly IWeatherApiClient _weatherApiClient;
    private readonly WeatherContext _context;
    private readonly IMapper _mapper;

    public WeatherService(
        IWeatherApiClient weatherApiClient, 
        WeatherContext context, 
        IMapper mapper)
    {
        _weatherApiClient = weatherApiClient;
        _context = context;
        _mapper = mapper;
    }
    public async Task<LocationDto?> GetForecastAsync(int locationId)
    {
        var location = await _context.Locations
            .Include(x => x.WeatherForecasts)
            .FirstOrDefaultAsync(x => x.Id == locationId);

        if (location is null) return null;
        
        var externalWeather = await _weatherApiClient.FetchWeatherAsync(location.Latitude, location.Longitude);
        var weather = _mapper.Map<List<WeatherForecast>>(externalWeather);

        location.WeatherForecasts = weather;
        await _context.SaveChangesAsync();

        return _mapper.Map<LocationDto>(location);
    }

    public async Task<PagedResult<LocationDto>> GetAllLocations(int pageNumber, int pageSize)
    {
        var query = _context.Locations;
        var pageData = await query
            .OrderBy(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        var totalCount = await query.CountAsync();
        
        var pagedResult = new PagedResult<LocationDto>(
            _mapper.Map<List<LocationDto>>(pageData), 
            totalCount, 
            pageNumber, 
            pageSize);
        
        return pagedResult;
    }

    public async Task DeleteLocation(int locationId)
    {
        var location = await _context.Locations
            .Include(x => x.WeatherForecasts)
            .FirstOrDefaultAsync(x => x.Id == locationId);

        if (location is null) return;

        _context.Remove(locationId);
        await _context.SaveChangesAsync();

    }

    public async Task AddLocation(LocationDto locationDto)
    {
        var existingLocation = await _context.Locations
            .Where(x => x.Latitude.Equals(locationDto.Latitude))
            .Where(x => x.Longitude.Equals(locationDto.Longitude))
            .AnyAsync();

        if (existingLocation)
        {
            throw new ArgumentException("Location already exists");
        }

        _context.Locations.Add(_mapper.Map<Location>(locationDto));
        await _context.SaveChangesAsync();
    }
}