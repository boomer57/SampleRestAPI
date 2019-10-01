using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SampleRestAPI.API.Domain.Models;
using SampleRestAPI.API.Domain.Models.Queries;
using SampleRestAPI.API.Domain.Repositories;
using SampleRestAPI.API.Persistence.Contexts;

namespace SampleRestAPI.API.Persistence.Repositories
{
    public class RatingRepository : BaseRepository, IRatingRepository
    {
        public RatingRepository(AppDbContext context) : base(context) { }

        public async Task<QueryResult<Rating>> ListAsync(RatingsQuery query)
        {
            IQueryable<Rating> queryable = _context.Ratings
                                                    .Include(p => p.MovieId)
                                                    .AsNoTracking();   // AsNoTracking tells EF Core to skip tracking changes on listed entities improving performance.

            if (query.MovieId.HasValue && query.MovieId > 0)
            {
                queryable = queryable.Where(p => p.MovieId == query.MovieId);    
            }
                        
            int totalItems = await queryable.CountAsync(); // Count all items in the database for the given query, returned as part of the pagination data.

            List<Rating> ratings = await queryable.Skip((query.Page - query.ItemsPerPage) * query.ItemsPerPage)
                                                    .Take(query.ItemsPerPage)
                                                    .ToListAsync();  // Calculate how many items to skip, according to the current page and amount of items per page, then return only the amount of desired items. The methods "Skip" and "Take" do the trick here.

            return new QueryResult<Rating>   // Return a query result, containing all items and the amount of items in the database (necessary for client calculations of pages).
            {
                Items = ratings,
                TotalItems = totalItems,
            };
        }

        public async Task<Rating> FindByIdAsync(int id)
        {
            return await _context.Ratings.FindAsync(id);
        }

        public async Task AddAsync(Rating rating)
        {
            await _context.Ratings.AddAsync(rating);
        }

        public void Update(Rating rating)
        {
            _context.Ratings.Update(rating);
        }

        public void Remove(Rating rating)
        {
            _context.Ratings.Remove(rating);
        }


    }
}