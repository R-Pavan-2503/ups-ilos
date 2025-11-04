using Microsoft.EntityFrameworkCore;

// This is the namespace for our infrastructure layer
namespace ILOS.Infrastructure.Persistence
{
    // This is our "bridge" class. It inherits from EF Core's DbContext
    public class ApplicationDbContext : DbContext
    {
        // This constructor is required by EF Core. It allows us to
        // pass in configuration (like the connection string) from our
        // API project.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // We will add our tables here later as DbSet properties.
        // For example:
        // public DbSet<Package> Packages { get; set; }
        // public DbSet<Route> Routes { get; set; }
    }
}