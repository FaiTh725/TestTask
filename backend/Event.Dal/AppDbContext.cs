using Event.Dal.Configuration;
using Event.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Application.Shared.Exceptions;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

namespace Event.Dal
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

        public DbSet<EventEntity> Events {  get; set; }

        public DbSet<EventMember> Members {  get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                configuration.GetConnectionString("SQLServerConnection") ??
                throw new AplicationConfigurationException("SqlServer connection string"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EventEntityConfiguration());
            modelBuilder.ApplyConfiguration(new EventMemberConfiguration());
        }
    }
}
