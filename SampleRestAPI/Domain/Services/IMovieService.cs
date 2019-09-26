using System.Collections.Generic;
using System.Threading.Tasks;
using SampleRestAPI.API.Domain.Models;
using SampleRestAPI.API.Domain.Services.Communication;

namespace SampleRestAPI.API.Domain.Services
{
    public interface IMovieService
    {
         Task<IEnumerable<Movie>> ListAsync();
         Task<MovieResponse> SaveAsync(Movie movie);
         Task<MovieResponse> UpdateAsync(int id, Movie movie);
         Task<MovieResponse> DeleteAsync(int id);
    }
}