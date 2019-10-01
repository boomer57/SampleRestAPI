using System.Collections.Generic;
using System.Threading.Tasks;
using SampleRestAPI.API.Domain.Models;
using SampleRestAPI.API.Domain.Models.Queries;

namespace SampleRestAPI.API.Domain.Repositories
{
    public interface IRatingRepository
    {
        Task<QueryResult<Rating>> ListAsync();
        Task AddAsync(Rating rating);
        Task<Rating> FindByIdAsync(int id);
        void Update(Rating rating);
        void Remove(Rating rating);
    }
}