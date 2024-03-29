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
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<QueryResult<User>> ListAsync(UsersQuery query)
        {
            IQueryable<User> queryable = _context.Users
                                                    .AsNoTracking();
           
            // Here I count all items present in the database for the given query, to return as part of the pagination data.
            int totalItems = await queryable.CountAsync();
            
            // Here I apply a simple calculation to skip a given number of items, according to the current page and amount of items per page,
            // and them I return only the amount of desired items. The methods "Skip" and "Take" do the trick here.
            List<User> users = await queryable.Skip((query.Page - query.ItemsPerPage) * query.ItemsPerPage)
                                                    .Take(query.ItemsPerPage)
                                                    .ToListAsync();

            // Finally I return a query result, containing all items and the amount of items in the database (necessary for client calculations of pages).
            return new QueryResult<User>
            {
                Items = users,
                TotalItems = totalItems,
            };
        }

        public async Task<User> FindByIdAsync(int id)
        {
            return await _context.Users
                                 .FirstOrDefaultAsync(p => p.Id == id); 
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        public void Remove(User user)
        {
            _context.Users.Remove(user);
        }
    }
}