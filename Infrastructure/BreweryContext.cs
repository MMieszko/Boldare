using Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

internal class BreweryContext(DbContextOptions<BreweryContext> options) : DbContext(options)
{
    public DbSet<Brewery> Breweries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BreweryContext).Assembly);
    }
}
