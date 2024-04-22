using System.Diagnostics.CodeAnalysis;
using WeatherApi.Data;

namespace WeatherApi.Configuration
{
    [SuppressMessage("ReSharper", "SA1649")]
    public interface IDatabaseMigrator
    {
        void Migrate();
    }

    public class DatabaseMigrator : IDatabaseMigrator
    {
        private readonly WeatherContext _weatherContext;

        public DatabaseMigrator(WeatherContext weatherContext)
        {
            _weatherContext = weatherContext;
        }

        public void Migrate()
        {
            _weatherContext.Migrate();
        }
    }
}
