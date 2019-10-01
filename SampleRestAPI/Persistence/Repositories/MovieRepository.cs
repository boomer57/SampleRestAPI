using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SampleRestAPI.API.Domain.Models;
using SampleRestAPI.API.Domain.Repositories;
using SampleRestAPI.API.Persistence.Contexts;

namespace SampleRestAPI.API.Persistence.Repositories
{
    public class MovieRepository : BaseRepository, IMovieRepository
    {
        public MovieRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Movie>> ListAsync()
        {
            return await _context.Movies
                                 .AsNoTracking()   // AsNoTracking tells EF Core to skip tracking changes on listed entities improving performance.
                                 .ToListAsync();
        }

        public async Task AddAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
        }

        public async Task<Movie> FindByIdAsync(int id)
        {
            return await _context.Movies.FindAsync(id);
        }

        public void Update(Movie movie)
        {
            _context.Movies.Update(movie);
        }

        public void Remove(Movie movie)
        {
            _context.Movies.Remove(movie);
        }
    }
}