using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using Fleet_Management_system.Models;

namespace Fleet_Management_system.Data
{
    public class Contextdata : DbContext
    {
        protected readonly IConfiguration Configuration;

        public Contextdata(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
        }
        public DbSet<Fleet_Management_system.Models.Vehicle> Vehicle { get; set; } = default!;
        public DbSet<Fleet_Management_system.Models.Routehistory> Routehistory { get; set; } = default!;
        public DbSet<Fleet_Management_system.Models.Rectanglegeofence> Rectanglegeofence { get; set; } = default!;
        public DbSet<Fleet_Management_system.Models.Polygongeofence> Polygongeofence { get; set; } = default!;
        public DbSet<Fleet_Management_system.Models.Geofence> Geofence { get; set; } = default!;
        public DbSet<Fleet_Management_system.Models.Driver> Driver { get; set; } = default!;
        public DbSet<Fleet_Management_system.Models.Circlegeofence> Circlegeofence { get; set; } = default!;
        public DbSet<Fleet_Management_system.Models.Vehiclesinformation> Vehiclesinformation { get; set; } = default!;

    }
}

