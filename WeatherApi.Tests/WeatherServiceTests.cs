using System.Net;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using WeatherApi.BusinessLogic.Abstraction.Models;
using WeatherApi.BusinessLogic.Clients.WeatherApi;
using WeatherApi.BusinessLogic.Mappings;
using WeatherApi.BusinessLogic.Services;
using WeatherApi.Data;
using WeatherApi.Data.Entities;
using WeatherApi.Tests;

[TestFixture]
public class WeatherServiceTests
{
    private WeatherContext _weatherContext;
    private IMapper _mapper;
    private WeatherService _weatherService;
    private Mock<IWeatherApiClient> _weatherApiClientMock;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<WeatherContext>()
            .UseInMemoryDatabase(databaseName: "WeatherTestDb" + Guid.NewGuid())
            .Options;
        _weatherContext = new WeatherContext(options);

        _weatherApiClientMock = new Mock<IWeatherApiClient>();

        var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new WeatherForecastProfile()); });
        _mapper = mappingConfig.CreateMapper();

        _weatherService = new WeatherService(_weatherApiClientMock.Object, _weatherContext, _mapper);
    }

    [TearDown]
    public void Teardown()
    {
        _weatherContext.Database.EnsureDeleted(); // This deletes the in-memory database
        _weatherContext.Dispose(); // Dispose the context to free up resources
    }


    [Test]
    public async Task GetForecastAsync_LocationExists_ReturnsLocationDto()
    {
        // Arrange
        _weatherContext.Locations.AddRange(
            new Location { Latitude = 30.0, Longitude = 50.0 }
        );
        _weatherContext.SaveChanges();
        var locationId = _weatherContext.Locations.Select(x => x.Id).First();

        _weatherApiClientMock.Setup(x => x.FetchWeatherAsync(It.IsAny<double>(), It.IsAny<double>())).ReturnsAsync(WeatherClientHelper.GetSampleData());
        // Act
        var result = await _weatherService.GetForecastAsync(locationId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LocationDto>();
    }

    [Test]
    public async Task AddLocation_NewLocation_AddsLocation()
    {
        // Arrange
        var newLocationDto = new LocationDto { Latitude = 25.0, Longitude = 45.0 };

        // Act
        await _weatherService.AddLocation(newLocationDto);

        var location = await _weatherContext.Locations.FirstOrDefaultAsync(l => l.Latitude == 25.0 && l.Longitude == 45.0);

        // Assert
        location.Should().NotBeNull();
        location.Latitude.Should().Be(25.0);
        location.Longitude.Should().Be(45.0);
    }

    [Test]
    public void AddLocation_DuplicateLocation_ThrowsArgumentException()
    {
        // Arrange
        var duplicateLocationDto = new LocationDto { Latitude = 20.0, Longitude = 40.0 }; // Coordinates already exist in the database

        // Act
        Func<Task> act = async () => await _weatherService.AddLocation(duplicateLocationDto);

        // Assert
        act.Should().ThrowAsync<ArgumentException>().WithMessage("Location already exists");
    }

    [Test]
    public async Task DeleteLocation_ExistingLocation_DeletesLocation()
    {
        // Arrange
        var locationId = 1; // Assuming a location with ID 1 exists from setup

        // Act
        await _weatherService.DeleteLocation(locationId);

        var location = await _weatherContext.Locations.FindAsync(locationId);

        // Assert
        location.Should().BeNull();
    }

    [Test]
    public async Task GetAllLocations_WhenCalled_ReturnsPagedLocations()
    {
        // Arrange
        // Add more sample data if needed
        _weatherContext.Locations.AddRange(
            new Location { Latitude = 30.0, Longitude = 50.0 },
            new Location { Latitude = 35.0, Longitude = 55.0 }
        );
        _weatherContext.SaveChanges();

        // Act
        var result = await _weatherService.GetAllLocations(1, 2);

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().HaveCount(2);
        result.Pagination.TotalPages.Should().Be(1);
        result.Pagination.PageSize.Should().Be(2);
        result.Pagination.TotalItems.Should().Be(2); 
    }
}