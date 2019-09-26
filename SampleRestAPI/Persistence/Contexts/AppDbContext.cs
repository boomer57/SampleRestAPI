using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.ValueGeneration.Internal;
using SampleRestAPI.API.Domain.Models;

namespace SampleRestAPI.API.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Movie>().ToTable("Movies");
            builder.Entity<Movie>().HasKey(p => p.Id);
            builder.Entity<Movie>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd().HasValueGenerator<InMemoryIntegerValueGenerator<int>>();
            builder.Entity<Movie>().Property(p => p.Title).IsRequired().HasMaxLength(30);
            builder.Entity<Movie>().HasMany(p => p.Users).WithOne(p => p.Movie).HasForeignKey(p => p.MovieId);

            builder.Entity<Movie>().HasData
            (
                new Movie { Id = 100, Title = "Fruits and Vegetables" }, // Id set manually due to in-memory provider
                new Movie { Id = 101, Title = "Dairy" }
            );

            builder.Entity<User>().ToTable("Users");
            builder.Entity<User>().HasKey(p => p.Id);
            builder.Entity<User>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<User>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<User>().Property(p => p.QuantityInPackage).IsRequired();
            builder.Entity<User>().Property(p => p.UnitOfMeasurement).IsRequired();

            builder.Entity<User>().HasData
            (
                new User
                {
                    Id = 100,
                    Name = "Apple",
                    QuantityInPackage = 1,
                    UnitOfMeasurement = EUnitOfMeasurement.Unity,
                    MovieId = 100
                },
                new User
                {
                    Id = 101,
                    Name = "Milk",
                    QuantityInPackage = 2,
                    UnitOfMeasurement = EUnitOfMeasurement.Liter,
                    MovieId = 101,
                }
            );
        }
    }
}