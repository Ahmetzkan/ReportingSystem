using Core.Entities;
using Entities.Concretes;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Data;
using System.Reflection;

namespace DataAccess.Contexts
{
    public class ReportingSystemContext : DbContext
    {
        protected IConfiguration _configuration { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Entities.Concretes.Task> Tasks { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        public ReportingSystemContext(DbContextOptions dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
        {
            _configuration = configuration;
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ReportingSystemDb;integrated security=true;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}
