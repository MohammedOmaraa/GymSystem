using GymSystem.Configurations;
using GymSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Contexts
{
    public class GymDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;database=GymSystem;Trusted_Connection=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PlanConfigurations());
        }

        public DbSet<Plan> Plans { get; set; }
    }
}
