using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleRestAPI.API.Domain.Models;
using SampleRestAPI.API.Domain.Models.Queries;
using SampleRestAPI.API.Domain.Services;
using SampleRestAPI.API.Resources;

namespace SampleRestAPI.API.Controllers
{
    [Route("/api/rating")]
    [Produces("application/json")]
    [ApiController]
    public class RatingController : Controller
    {
        private readonly IRatingService _ratingService;
        private readonly IMapper _mapper;

        public RatingController(IRatingService ratingService, IMapper mapper)
        {
            _ratingService = ratingService;
            _mapper = mapper;
        }

        /// <summary>
        /// Saves a new Rating.
        /// </summary>
        /// <param name="resource">Rating data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(RatingResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PostAsync([FromBody] SaveRatingResource resource)
        {
            var rating = _mapper.Map<SaveRatingResource, Rating>(resource);
            var result = await _ratingService.SaveAsync(rating);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var ratingResource = _mapper.Map<Rating, RatingResource>(result.Resource);
            return Ok(ratingResource);
        }

        /// <summary>
        /// Updates an existing rating according to an identifier.
        /// </summary>
        /// <param name="id">Rating identifier.</param>
        /// <param name="resource">Updated rating data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RatingResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveRatingResource resource)
        {
            var rating = _mapper.Map<SaveRatingResource, Rating>(resource);
            var result = await _ratingService.UpdateAsync(id, rating);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var ratingResource = _mapper.Map<Rating, RatingResource>(result.Resource);
            return Ok(ratingResource);
        }

        /// <summary>
        /// Deletes a given Rating according to an identifier.
        /// </summary>
        /// <param name="id">Rating identifier.</param>
        /// <returns>Response for the request.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(RatingResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _ratingService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var ratingResource = _mapper.Map<Rating, RatingResource>(result.Resource);
            return Ok(ratingResource);
        }
    }
}