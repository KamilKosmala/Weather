using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WeatherApi.BusinessLogic.Abstraction.Interfaces;
using WeatherApi.BusinessLogic.Abstraction.Models;
using WeatherApi.Helpers;
using WeatherApi.Models;

namespace WeatherApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;
    private readonly IMapper _mapper;

    public WeatherController(
        IWeatherService weatherService,
        IMapper mapper)
    {
        _weatherService = weatherService;
        _mapper = mapper;
    }

    [HttpGet("{locationId}")]
    public async Task<IActionResult> GetWeatherForecast(int locationId)
    {
        var location = await _weatherService.GetForecastAsync(locationId);

        if (location is null)
        {
            return NotFound();
        }

        var response = _mapper.Map<LocationResponse>(location);
        return Ok(response);
    }

    [HttpGet("location")]
    public async Task<IActionResult> GetLocations([FromQuery]int? pageNumber = 1, [FromQuery]int? pageSize = 10 )
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return BadRequest("pageNumber and pageSize values should be greater than 0");
        }
        var pagedResult = await _weatherService.GetAllLocations(pageNumber!.Value, pageSize!.Value);

        
        var pagedResponse = new PagedResult<LocationListResponse>(
            _mapper.Map<List<LocationListResponse>>(pagedResult.Data), pagedResult.Pagination);
        
        return Ok(pagedResponse);
    }
    
    [HttpPost("location")]
    public async Task<IActionResult> AddLocation([FromBody] LocationRequest request)
    {
        if (!GeoCoordinateValidator.IsValidCoordinate(request.Latitude, request.Longitude))
        {
            return BadRequest("Invalid coordinates");
        }
        try
        {
            var location = _mapper.Map<LocationDto>(request);
            await _weatherService.AddLocation(location);
        }
        catch (ArgumentException e)
        {
            return Conflict(e.Message);
        }
      
        return Ok("Location added successfully.");
    }

    [HttpDelete("location/{locationId}")]
    public async Task<IActionResult> DeleteLocation(int locationId)
    {
        await _weatherService.DeleteLocation(locationId);
        return NoContent();
    }
}