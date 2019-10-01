using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleRestAPI.API.Domain.Models;
using SampleRestAPI.API.Domain.Models.Queries;
using SampleRestAPI.API.Domain.Services;
using SampleRestAPI.API.Resources;

namespace SampleRestAPI.API.Controllers
{
    [Route("/api/movie")]
    [Produces("application/json")]
    [ApiController]
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IRatingService _ratingService;
        private readonly IMapper _mapper;

        public MovieController(IMovieService movieService, IMapper mapper)
        {
            _movieService = movieService;
            _mapper = mapper;
        }

        /// <summary>
        /// Lists all Movies.
        /// </summary>
        /// <returns>List of movies</returns>
        [HttpGet]
        [Route("/api/movie/{id}/{releaseYear}/{genre}")]
        [ProducesResponseType(typeof(IEnumerable<MovieResource>), 200)]
        //[ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<ActionResult<IEnumerable<MovieResource>>> ListAsync(int? id = 0, int? releaseYear = 0, string genre = "")
        {
            if (id == 0 || releaseYear == 0 || string.IsNullOrWhiteSpace(genre))
            {
                return BadRequest("Bad Route Parameters");
            }

            var movies = await _movieService.ListAsync();

            if (id != null && id > 0)
            {
                movies = movies.Where(m => m.Id == id);
            }
            if (movies.Count() == 0)
            {
                string msg = $"Bad Request for id = {id}";
                return BadRequest(msg);
            }
            var resources = _mapper.Map<IEnumerable<Movie>, IEnumerable<MovieResource>>(movies);

            return Ok(resources);
        }

        /// <summary>
        /// Lists Top Rated Movies.
        /// </summary>
        /// <returns>List of Movies.</returns>
        [HttpGet]
        [Route("/api/movie/globalTop5")]
        public async Task<ActionResult<IEnumerable<Movie>>> ListGlobalTop5Async()
        {
            //var ratingsQuery = _mapper.Map<RatingsQueryResource, RatingsQuery>(query);
            var queryResult = await _movieService.ListGlobalTopAsync(5);

            var resource = _mapper.Map<QueryResult<Rating>, IEnumerable<RatingResource>>(queryResult);
            return Ok(resource);
        }

        /// <summary>
        /// Lists Top Rated Movies for a User.
        /// </summary>
        /// <returns>List of Movies.</returns>
        [HttpGet]
        [Route("/api/movie/userTop5/{userId}")]
        public async Task<ActionResult<IEnumerable<Movie>>> ListUserTop5Async(int userId)
        {
            var ratingsQuery = _mapper.Map<RatingsQueryResource, RatingsQuery>(query);
            var queryResult = await _movieService.ListUserTopAsync(5, userId);

            var resource = _mapper.Map<QueryResult<Rating>, QueryResultResource<RatingResource>>(queryResult);
            return Ok(resource);
        }



        /// <summary>
        /// Saves a new Movie.
        /// </summary>
        /// <param name="resource">Movie data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(MovieResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PostAsync([FromBody] SaveMovieResource resource)
        {
            var movie = _mapper.Map<SaveMovieResource, Movie>(resource);
            var result = await _movieService.SaveAsync(movie);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var movieResource = _mapper.Map<Movie, MovieResource>(result.Resource);
            return Ok(movieResource);
        }
        /// <summary>
        /// Updates an existing movie according to an identifier.
        /// </summary>
        /// <param name="id">Movie identifier.</param>
        /// <param name="resource">Updated movie data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MovieResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveMovieResource resource)
        {
            var movie = _mapper.Map<SaveMovieResource, Movie>(resource);
            var result = await _movieService.UpdateAsync(id, movie);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var movieResource = _mapper.Map<Movie, MovieResource>(result.Resource);
            return Ok(movieResource);
        }

        /// <summary>
        /// Deletes a given Movie according to an identifier.
        /// </summary>
        /// <param name="id">Movie identifier.</param>
        /// <returns>Response for the request.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MovieResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _movieService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var movieResource = _mapper.Map<Movie, MovieResource>(result.Resource);
            return Ok(movieResource);
        }
    }
}