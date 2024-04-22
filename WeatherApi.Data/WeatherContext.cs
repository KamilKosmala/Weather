using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WeatherApi.Data.Configurations;
using WeatherApi.Data.Entities;

namespace WeatherApi.Data;

public class WeatherContext : DbContext
{
    public WeatherContext(DbContextOptions<WeatherContext> options) : base(options)
    {
    }

    public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    public DbSet<Location> Locations { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries().Where(x => x.Entity.GetType().GetProperty("Created") != null))
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Property("Created").CurrentValue = DateTimeOffset.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Property("Created").IsModified = false;
                    break;
            }
        }

        foreach (var entry in ChangeTracker.Entries().Where(e => e.Entity.GetType().GetProperty("Updated") != null &&
                                                                 (e.State == EntityState.Modified ||
                                                                  e.State == EntityState.Added)))
        {
            entry.Property("Updated").CurrentValue = DateTimeOffset.UtcNow;
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    public void Migrate()
    {
        Database.Migrate();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IDataAssembly).Assembly);
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IBaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTimeOffset>(nameof(IBaseEntity.Created))
                    .HasDefaultValueSql("getutcdate()");
            }
        }

        var entityTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x
                => x.GetTypeInfo().IsSubclassOf(typeof(IBaseEntity)) && !x.GetTypeInfo().IsAbstract);

        foreach (var type in entityTypes)
        {
            modelBuilder.Entity(type);
        }

        var builderTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => typeof(IModelBuilder).IsAssignableFrom(x) && !x.IsInterface);

        foreach (var builderType in builderTypes)
        {
            var builder = (IModelBuilder)Activator.CreateInstance(builderType)!;
            builder.Build(modelBuilder);
        }
    }
}