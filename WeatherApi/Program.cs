using Microsoft.EntityFrameworkCore;
using WeatherApi.BusinessLogic.Abstraction.Interfaces;
using WeatherApi.BusinessLogic.Clients.WeatherApi;
using WeatherApi.BusinessLogic.Mappings;
using WeatherApi.BusinessLogic.Services;
using WeatherApi.Configuration;
using WeatherApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IWeatherApiClient, WeatherApiClient>();

builder.Services.AddDbContext<WeatherContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WeatherDatabase")));

builder.Services.AddAutoMapper([typeof(WeatherForecastProfile),typeof(WeatherApi.Mappings.WeatherForecastProfile)]);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
DatabaseBuilder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
