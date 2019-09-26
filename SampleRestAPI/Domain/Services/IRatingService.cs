using System.Collections.Generic;
using System.Threading.Tasks;
using SampleRestAPI.API.Domain.Models;
using SampleRestAPI.API.Domain.Models.Queries;
using SampleRestAPI.API.Domain.Services.Communication;

namespace SampleRestAPI.API.Domain.Services
{
    public interface IRatingService
    {
         Task<QueryResult<Rating>> ListAsync(RatingsQuery ratingsQuery);
         Task<RatingResponse> SaveAsync(Rating rating);
         Task<RatingResponse> UpdateAsync(int id, Rating rating);
         Task<RatingResponse> DeleteAsync(int id);
    }
}