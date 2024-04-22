using Microsoft.EntityFrameworkCore;
using WeatherApi.Data;

namespace WeatherApi.Configuration
{
    public static class DatabaseBuilder
    {
        public static void Build()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";
            var contentRootPath = Directory.GetCurrentDirectory();

            var builder = new ConfigurationBuilder()
                .SetBasePath(contentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true);

            builder.AddEnvironmentVariables();
            var configuration = builder.Build();

            // setup DI
            IServiceCollection services = new ServiceCollection();
            services.AddScoped<IDatabaseMigrator, DatabaseMigrator>();
            services.AddDbContext<WeatherContext>(options =>
                options
                    .UseSqlServer(configuration.GetConnectionString("WeatherDatabase")));
            var serviceProvider = services.BuildServiceProvider();
            var databaseMigrator = serviceProvider.GetRequiredService<IDatabaseMigrator>();

            databaseMigrator.Migrate();
        }
    }
}