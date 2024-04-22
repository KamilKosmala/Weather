using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeatherApi.Data.Entities;

namespace WeatherApi.Data.Configurations;

public class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T>
    where T : BaseEntity, IBaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder
            .HasKey(c => c.Id);

        builder
            .Property(entity => entity.Created)
            .HasDefaultValueSql("getutcdate()");

        builder
            .Property(entity => entity.Updated)
            .HasDefaultValueSql("getutcdate()")
            .ValueGeneratedOnUpdate();
    }
}