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
    [Route("/api/user")]
    [Produces("application/json")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Lists all existing users.
        /// </summary>
        /// <returns>List of users.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(QueryResultResource<UserResource>), 200)]
        public async Task<QueryResultResource<UserResource>> ListAsync([FromQuery] UsersQueryResource query)
        {
            var usersQuery = _mapper.Map<UsersQueryResource, UsersQuery>(query);
            var queryResult = await _userService.ListAsync(usersQuery);

            var resource = _mapper.Map<QueryResult<User>, QueryResultResource<UserResource>>(queryResult);
            return resource;
        }

        /// <summary>
        /// Saves a new user.
        /// </summary>
        /// <param name="resource">User data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UserResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PostAsync([FromBody] SaveUserResource resource)
        {
            var user = _mapper.Map<SaveUserResource, User>(resource);
            var result = await _userService.SaveAsync(user);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var userResource = _mapper.Map<User, UserResource>(result.Resource);
            return Ok(userResource);
        }

        /// <summary>
        /// Updates an existing user according to an identifier.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <param name="resource">User data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveUserResource resource)
        {
            var user = _mapper.Map<SaveUserResource, User>(resource);
            var result = await _userService.UpdateAsync(id, user);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var userResource = _mapper.Map<User, UserResource>(result.Resource);
            return Ok(userResource);
        }

        /// <summary>
        /// Deletes a given user according to an identifier.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>Response for the request.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(UserResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _userService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var movieResource = _mapper.Map<User, UserResource>(result.Resource);
            return Ok(movieResource);
        }
    }
}