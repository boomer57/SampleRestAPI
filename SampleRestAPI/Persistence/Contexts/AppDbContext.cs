using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.ValueGeneration.Internal;
using SampleRestAPI.API.Domain.Models;

namespace SampleRestAPI.API.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Rating> Ratings { get; set; }
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
            builder.Entity<Movie>()
                .HasMany(p => p.Users)
                .WithOne(p => p.Movie)
                .HasForeignKey(p => p.Movie);

            //builder.Entity<Movie>().HasData
            //(
            //    new Movie { Id = 100, Title = "The Godfather" }, // Id set manually due to in-memory provider
            //    new Movie { Id = 101, Title = "The Big Bang Theory" }
            //);

            builder.Entity<User>().ToTable("Users");
            builder.Entity<User>().HasKey(p => p.Id);
            builder.Entity<User>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<User>().Property(p => p.FirstName).IsRequired().HasMaxLength(50);
            builder.Entity<User>().Property(p => p.LastName).IsRequired().HasMaxLength(50);

            //builder.Entity<User>().HasData
            //(
            //    new User
            //    {
            //        Id = 100,
            //        FirstName = "Neal",
            //        LastName = "Miller"
            //    },
            //    new User
            //    {
            //        Id = 101,
            //        FirstName = "John",
            //        LastName = "Smith"
            //    }
            //);

            builder.Entity<Rating>().ToTable("Ratings");
            builder.Entity<Rating > ().HasKey(p => p.Id);
            builder.Entity<Rating>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Rating>().Property(p => p.RatingValue).IsRequired();
            builder.Entity<Rating>().Property(p => p.MovieId).IsRequired();
            builder.Entity<Rating>().Property(p => p.UserId).IsRequired();

            //builder.Entity<Rating>().HasData
            //(
            //    new Rating
            //    {
            //        Id = 100,
            //        RatingValue = 5,
            //        MovieId = 100,
            //        UserId = 100
            //    },
            //    new Rating
            //    {
            //        Id = 101,
            //        RatingValue = 5,
            //        MovieId = 100,
            //        UserId = 100
            //    }
            //);
        }
    }
}