using SampleRestAPI.API.Domain.Models;
using SampleRestAPI.API.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleRestAPI.Persistence
{
    public class TestDataSeeder
    {
        private readonly AppDbContext _context;
        public TestDataSeeder(AppDbContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            _context.Movies.Add(new Movie { Id = 100, Title = "The Godfather", Genres = "Crime", RunningTime = 200, YearReleased = 1972 }); // Id set manually due to in-memory provider
            _context.Movies.Add(new Movie { Id = 101, Title = "The Big Bang Theory", Genres = "Comedy", RunningTime = 30, YearReleased = 2009 });
            _context.Movies.Add(new Movie { Id = 102, Title = "Blues Brothers", Genres = "Comedy", RunningTime = 150, YearReleased = 1979 });
            _context.Movies.Add(new Movie { Id = 103, Title = "The Avengers Endgame", Genres = "Superhero", RunningTime = 220, YearReleased = 2019 });
            _context.Movies.Add(new Movie { Id = 104, Title = "On The Basis of Sex", Genres = "BioPic", RunningTime = 140, YearReleased = 2019 });
            _context.Movies.Add(new Movie { Id = 105, Title = "Captain Marvel", Genres = "Superhero", RunningTime = 180, YearReleased = 2019 });
            _context.Movies.Add(new Movie { Id = 106, Title = "The Pink Panther", Genres = "Comedy", RunningTime = 140, YearReleased = 2008 });

            _context.Users.Add(new User { Id = 100, FirstName = "Neal", LastName = "Miller" });
            _context.Users.Add(new User { Id = 101, FirstName = "John", LastName = "Smith" });

            _context.Ratings.Add(new Rating { Id = 100, RatingValue = 5, MovieId = 100, UserId = 100 });
            _context.Ratings.Add(new Rating { Id = 101, RatingValue = 4, MovieId = 101, UserId = 100 });
            _context.Ratings.Add(new Rating { Id = 102, RatingValue = 5, MovieId = 102, UserId = 100 });
            _context.Ratings.Add(new Rating { Id = 103, RatingValue = 4, MovieId = 103, UserId = 100 });
            _context.Ratings.Add(new Rating { Id = 104, RatingValue = 3, MovieId = 104, UserId = 100 });
            _context.Ratings.Add(new Rating { Id = 105, RatingValue = 2, MovieId = 105, UserId = 100 });
            _context.Ratings.Add(new Rating { Id = 106, RatingValue = 3, MovieId = 106, UserId = 100 });

            _context.Ratings.Add(new Rating { Id = 110, RatingValue = 2, MovieId = 100, UserId = 101 });
            _context.Ratings.Add(new Rating { Id = 111, RatingValue = 5, MovieId = 101, UserId = 101 });
            _context.Ratings.Add(new Rating { Id = 112, RatingValue = 1, MovieId = 102, UserId = 101 });
            _context.Ratings.Add(new Rating { Id = 113, RatingValue = 2, MovieId = 103, UserId = 101 });
            _context.Ratings.Add(new Rating { Id = 114, RatingValue = 5, MovieId = 104, UserId = 101 });
            _context.Ratings.Add(new Rating { Id = 115, RatingValue = 4, MovieId = 105, UserId = 101 });
            _context.Ratings.Add(new Rating { Id = 116, RatingValue = 5, MovieId = 106, UserId = 101 });

            _context.SaveChanges();
        }
    }
}
