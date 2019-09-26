using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleRestAPI.API.Domain.Models;
using SampleRestAPI.API.Domain.Services;
using SampleRestAPI.API.Resources;

namespace SampleRestAPI.API.Controllers
{
    [Route("/api/movies")]
    [Produces("application/json")]
    [ApiController]
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IMapper _mapper;

        public MoviesController(IMovieService movieService, IMapper mapper)
        {
            _movieService = movieService;
            _mapper = mapper;
        }

        /// <summary>
        /// Lists all Movies.
        /// </summary>
        /// <returns>List os Movies.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MovieResource>), 200)]
        public async Task<IEnumerable<MovieResource>> ListAsync()
        {
            var Movies = await _movieService.ListAsync();
            var resources = _mapper.Map<IEnumerable<Movie>, IEnumerable<MovieResource>>(Movies);

            return resources;
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