using Application.Shared.Exceptions;
using Authentication.Dal.Configurations;
using Authentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Authentication.Dal
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IConfiguration configuration) :
            base(options)
        {
            this.configuration = configuration;
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                configuration.GetConnectionString("SQLServerConnection") ??
                throw new ApplicationConfigurationException("SqlServer Connection String"));
        }
    }
}
