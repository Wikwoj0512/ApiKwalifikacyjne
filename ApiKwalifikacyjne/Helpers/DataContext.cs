namespace ApiKwalifikacyjne.Helpers;

using Microsoft.EntityFrameworkCore;
using Entities;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite(Configuration.GetConnectionString("ApiDatabase"));
    }

    public DbSet<Tag> Tags { get; set; }
}