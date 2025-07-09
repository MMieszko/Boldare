using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure;

internal class BreweryContextFactory : IDesignTimeDbContextFactory<BreweryContext>
{
    public BreweryContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BreweryContext>();
        var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        optionsBuilder.UseSqlite($"Data Source={path}/brewery.db");

        return new BreweryContext(optionsBuilder.Options);
    }
}