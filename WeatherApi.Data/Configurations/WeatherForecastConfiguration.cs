using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeatherApi.Data.Entities;

namespace WeatherApi.Data.Configurations;

public class WeatherForecastConfiguration : BaseEntityConfiguration<WeatherForecast>
{
    public override void Configure(EntityTypeBuilder<WeatherForecast> builder)
    {
        base.Configure(builder);
        
        builder
            .HasOne(wf => wf.Location)
            .WithMany(l => l.WeatherForecasts)
            .HasForeignKey(wf => wf.LocationId);

        
    }
}