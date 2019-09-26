using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using SampleRestAPI.API.Domain.Models;
using SampleRestAPI.API.Domain.Models.Queries;
using SampleRestAPI.API.Domain.Repositories;
using SampleRestAPI.API.Domain.Services;
using SampleRestAPI.API.Domain.Services.Communication;
using SampleRestAPI.API.Infrastructure;

namespace SampleRestAPI.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;

        public UserService(IUserRepository userRepository, IMovieRepository movieRepository, IUnitOfWork unitOfWork, IMemoryCache cache)
        {
            _userRepository = userRepository;
            _movieRepository = movieRepository;
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<QueryResult<User>> ListAsync(UsersQuery query)
        {
            // Here I list the query result from cache if they exist, but now the data can vary according to the Movie ID, page and amount of
            // items per page. I have to compose a cache to avoid returning wrong data.
            string cacheKey = GetCacheKeyForUsersQuery(query);
            
            var users = await _cache.GetOrCreateAsync(cacheKey, (entry) => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return _userRepository.ListAsync(query);
            });

            return users;
        }

        public async Task<UserResponse> SaveAsync(User user)
        {
            try
            {
                /*
                 Notice here we have to check if the Movie ID is valid before adding the user, to avoid errors.
                 You can create a method into the MovieService class to return the movie and inject the service here if you prefer, but 
                 it doesn't matter given the API scope.
                */
                var existingMovie = await _movieRepository.FindByIdAsync(user.MovieId);
                if (existingMovie == null)
                    return new UserResponse("Invalid Movie.");

                await _userRepository.AddAsync(user);
                await _unitOfWork.CompleteAsync();

                return new UserResponse(user);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new UserResponse($"An error occurred when saving the user: {ex.Message}");
            }
        }

        public async Task<UserResponse> UpdateAsync(int id, User user)
        {
            var existingUser = await _userRepository.FindByIdAsync(id);

            if (existingUser == null)
                return new UserResponse("User not found.");

            var existingMovie = await _movieRepository.FindByIdAsync(user.MovieId);
            if (existingMovie == null)
                return new UserResponse("Invalid Movie.");

            existingUser.Name = user.Name;
            existingUser.UnitOfMeasurement = user.UnitOfMeasurement;
            existingUser.QuantityInPackage = user.QuantityInPackage;
            existingUser.MovieId = user.MovieId;

            try
            {
                _userRepository.Update(existingUser);
                await _unitOfWork.CompleteAsync();

                return new UserResponse(existingUser);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new UserResponse($"An error occurred when updating the user: {ex.Message}");
            }
        }

        public async Task<UserResponse> DeleteAsync(int id)
        {
            var existingUser = await _userRepository.FindByIdAsync(id);

            if (existingUser == null)
                return new UserResponse("User not found.");

            try
            {
                _userRepository.Remove(existingUser);
                await _unitOfWork.CompleteAsync();

                return new UserResponse(existingUser);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new UserResponse($"An error occurred when deleting the user: {ex.Message}");
            }
        }

        private string GetCacheKeyForUsersQuery(UsersQuery query)
        {
            string key = CacheKeys.UsersList.ToString();
            
            if (query.MovieId.HasValue && query.MovieId > 0)
            {
                key = string.Concat(key, "_", query.MovieId.Value);
            }

            key = string.Concat(key, "_", query.Page, "_", query.ItemsPerPage);
            return key;
        }
    }
}