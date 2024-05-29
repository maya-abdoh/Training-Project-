using System;
using System.Collections.Generic;
using Fleet_Management_system.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_system.Data;

public partial class DemoContext2 : DbContext
{
    public DemoContext2()
    {
    }

    public DemoContext2(DbContextOptions<DemoContext2> options)
        : base(options)
    {
    }

    public virtual DbSet<Circlegeofence> Circlegeofences { get; set; }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<Geofence> Geofences { get; set; }

    public virtual DbSet<Polygongeofence> Polygongeofences { get; set; }

    public virtual DbSet<Rectanglegeofence> Rectanglegeofences { get; set; }

    public virtual DbSet<Routehistory> Routehistories { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<Vehiclesinformation> Vehiclesinformations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Circlegeofence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("circlegeofence_pkey");

            entity.HasOne(d => d.Geofence).WithMany(p => p.Circlegeofences).HasConstraintName("fk_circlegeofence");
        });

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.Driverid).HasName("driver_pkey");
        });

        modelBuilder.Entity<Driver>()
    .HasMany(d => d.Vehiclesinformations)
    .WithOne(v => v.Driver)
    .OnDelete(DeleteBehavior.Cascade); // Cascade delete configuration

        modelBuilder.Entity<Geofence>(entity =>
        {
            entity.HasKey(e => e.Geofenceid).HasName("geofences_pkey");
        });

        modelBuilder.Entity<Polygongeofence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("polygongeofence_pkey");

            entity.HasOne(d => d.Geofence).WithMany(p => p.Polygongeofences).HasConstraintName("fk_polygongeofence");
        });

        modelBuilder.Entity<Rectanglegeofence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("rectanglegeofence_pkey");

            entity.HasOne(d => d.Geofence).WithMany(p => p.Rectanglegeofences).HasConstraintName("fk_rectanglegeofence");
        });

        modelBuilder.Entity<Routehistory>(entity =>
        {
            entity.HasKey(e => e.Routehistoryid).HasName("routehistory_pkey");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.Routehistories).HasConstraintName("fk_vehiclehistory");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Vehicleid).HasName("vehicles_pkey");
        });

        modelBuilder.Entity<Vehicle>()
    .HasMany(v => v.Vehiclesinformations)
    .WithOne(i => i.Vehicle)
    .OnDelete(DeleteBehavior.Cascade); // Set cascade delete

        modelBuilder.Entity<Vehicle>()
            .HasMany(v => v.Routehistories)
            .WithOne(r => r.Vehicle)
            .OnDelete(DeleteBehavior.Cascade); // Set cascade delete

        modelBuilder.Entity<Geofence>()
    .HasMany(g => g.Rectanglegeofences)
    .WithOne(r => r.Geofence)
    .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<Vehiclesinformation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("vehiclesinformations_pkey");

            entity.HasOne(d => d.Driver).WithMany(p => p.Vehiclesinformations).HasConstraintName("fk_driver");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.Vehiclesinformations).HasConstraintName("fk_vehicle");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
