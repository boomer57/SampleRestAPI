using System;
using System.Collections.Generic;
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
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;

        public RatingService(IRatingRepository ratingRepository, IUnitOfWork unitOfWork, IMemoryCache cache)
        {
            _ratingRepository = ratingRepository;
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<QueryResult<Rating>> ListAsync(RatingsQuery query)
        {
            // Here I list the query result from cache if they exist, but now the data can vary according to the Movie ID, page and amount of
            // items per page. I have to compose a cache to avoid returning wrong data.
            string cacheKey = GetCacheKeyForRatingsQuery(query);

            var ratings = await _cache.GetOrCreateAsync(cacheKey, (entry) => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return _ratingRepository.ListAsync(query);
            });

            return ratings;
        }

        public async Task<RatingResponse> SaveAsync(Rating rating)
        {
            try
            {
                await _ratingRepository.AddAsync(rating);
                await _unitOfWork.CompleteAsync();

                return new RatingResponse(rating);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new RatingResponse($"An error occurred when saving the rating: {ex.Message}");
            }
        }

        public async Task<RatingResponse> UpdateAsync(int id, Rating rating)
        {
            var existingRating = await _ratingRepository.FindByIdAsync(id);

            if (existingRating == null)
                return new RatingResponse("Rating not found.");

            /*
                Notice here we have to check if the Movie ID is valid before adding the user, to avoid errors.
                You can create a method into the MovieService class to return the movie and inject the service here if you prefer, but 
                it doesn't matter given the API scope.
                ---> Same for following user id validity check.
            */
            var existingMovie = await _movieRepository.FindByIdAsync(rating.MovieId);
            if (existingMovie == null)
                return new RatingResponse("Invalid Movie.");

            var existingUser = await _userRepository.FindByIdAsync(rating.UserId);
            if (existingUser == null)
                return new RatingResponse("Invalid User.");

            existingRating.RatingValue = rating.RatingValue;

            try
            {
                _ratingRepository.Update(existingRating);
                await _unitOfWork.CompleteAsync();

                return new RatingResponse(existingRating);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new RatingResponse($"An error occurred when updating the rating: {ex.Message}");
            }
        }

        public async Task<RatingResponse> DeleteAsync(int id)
        {
            var existingRating = await _ratingRepository.FindByIdAsync(id);

            if (existingRating == null)
                return new RatingResponse("Rating not found.");

            try
            {
                _ratingRepository.Remove(existingRating);
                await _unitOfWork.CompleteAsync();

                return new RatingResponse(existingRating);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new RatingResponse($"An error occurred when deleting the rating: {ex.Message}");
            }
        }
        private string GetCacheKeyForRatingsQuery(RatingsQuery query)
        {
            string key = CacheKeys.RatingsList.ToString();

            if (query.MovieId.HasValue && query.MovieId > 0)
            {
                key = string.Concat(key, "_", query.MovieId.Value);
            }

            key = string.Concat(key, "_", query.Page, "_", query.ItemsPerPage);
            return key;
        }

    }
}