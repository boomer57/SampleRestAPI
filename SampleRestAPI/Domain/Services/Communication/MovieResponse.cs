using SampleRestAPI.API.Domain.Models;

namespace SampleRestAPI.API.Domain.Services.Communication
{
    public class MovieResponse : BaseResponse<Movie>
    {
        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="movie">Saved Movie.</param>
        /// <returns>Response.</returns>
        public MovieResponse(Movie movie) : base(movie)
        { }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public MovieResponse(string message) : base(message)
        { }
    }
}