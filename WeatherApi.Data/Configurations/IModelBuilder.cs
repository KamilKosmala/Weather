using Microsoft.EntityFrameworkCore;

namespace WeatherApi.Data.Configurations;

public interface IModelBuilder
{
    void Build(ModelBuilder modelBuilder);
}