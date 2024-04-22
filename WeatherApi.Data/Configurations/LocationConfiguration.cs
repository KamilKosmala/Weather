using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeatherApi.Data.Entities;

namespace WeatherApi.Data.Configurations;

public class LocationConfiguration : BaseEntityConfiguration<Location>
{
    public override void Configure(EntityTypeBuilder<Location> builder)
    {
        base.Configure(builder);
    }
}     