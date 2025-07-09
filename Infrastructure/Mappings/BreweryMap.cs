using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings;

internal class BreweryMap : IEntityTypeConfiguration<Brewery>
{
    public void Configure(EntityTypeBuilder<Brewery> builder)
    {
        builder.ToTable("Breweries");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.City);
        builder.Property(x => x.Name);

        builder.OwnsOne(x => x.GeoLocation, o =>
        {
            o.Property(g => g.Latitude).HasColumnName(nameof(GeoLocation.Latitude));
            o.Property(g => g.Longitude).HasColumnName(nameof(GeoLocation.Longitude));
        });
    }
}