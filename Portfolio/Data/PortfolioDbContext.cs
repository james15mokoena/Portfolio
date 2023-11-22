using Microsoft.EntityFrameworkCore;
using Portfolio.Models;

namespace Portfolio.Data
{
    public class PortfolioDbContext : DbContext
    {

        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : base(options) { }

        // Maps the Project model/class to a Project table in the database
        public DbSet<Project> Projects { get; set; }

        // Maps the Administrator model to the Administrator table in the database.
        public DbSet<Administrator> Administrators { get; set; }
    }
}
