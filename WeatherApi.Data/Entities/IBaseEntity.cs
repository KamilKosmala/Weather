namespace WeatherApi.Data.Entities;

public interface IBaseEntity
{
    public int Id { get; set; }

    public DateTimeOffset Created { get; set; }

    public DateTimeOffset? Updated { get; set; }
}