using System;
using System.Collections.Generic;
using System.Linq;
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
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;

        public MovieService(IMovieRepository movieRepository, IUnitOfWork unitOfWork, IMemoryCache cache)
        {
            _movieRepository = movieRepository;
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<IEnumerable<Movie>> ListAsync()
        {
            // Here I try to get the Movies list from the memory cache. If there is no data in cache, the anonymous method will be
            // called, setting the cache to expire one minute ahead and returning the Task that lists the Movies from the repository.
            var movies = await _cache.GetOrCreateAsync(CacheKeys.MoviesList, (entry) => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return _movieRepository.ListAsync();
            });
            
            return movies;
        }

        public async Task<IEnumerable<Movie>> ListGlobalTopAsync(int topHowMany)
        {
            // Here I list the query result from cache if they exist, but now the data can vary according to the Movie ID, page and amount of
            // items per page. I have to compose a cache to avoid returning wrong data.
            //string cacheKey = GetCacheKeyForRatingsQuery(query);

            //var ratings = await _cache.GetOrCreateAsync(cacheKey, (entry) => {
            //    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
            //    return _ratingRepository.ListAsync(query);
            //});

            var ratings = await _ratingRepository.ListAsync();
            var ratingsSortedListOut = ratings.Items.OrderBy(r => r.RatingValue).Take<Rating>(topHowMany);
            return ratingsSortedListOut;

            //return ratings;
        }

        public async Task<IEnumerable<Movie>> ListUserTopAsync(int topHowMany, int userId)
        {
            // Here I list the query result from cache if they exist, but now the data can vary according to the Movie ID, page and amount of
            // items per page. I have to compose a cache to avoid returning wrong data.
            //string cacheKey = GetCacheKeyForRatingsQuery(query);

            //var ratings = await _cache.GetOrCreateAsync(cacheKey, (entry) => {
            //    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
            //    return _ratingRepository.ListAsync(query);
            //});

            var ratings = _ratingRepository.ListAsync();
            var ratingsSortedListForUserOut = ratings.Items.Where(r => r.UserId == userId).OrderBy(r => r.RatingValue).Take<Rating>(topHowMany);
            return ratingsSortedListForUserOut;

            //return ratings;
        }



        public async Task<MovieResponse> SaveAsync(Movie movie)
        {
            try
            {
                await _movieRepository.AddAsync(movie);
                await _unitOfWork.CompleteAsync();

                return new MovieResponse(movie);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new MovieResponse($"An error occurred when saving the movie: {ex.Message}");
            }
        }

        public async Task<MovieResponse> UpdateAsync(int id, Movie movie)
        {
            var existingMovie = await _movieRepository.FindByIdAsync(id);

            if (existingMovie == null)
                return new MovieResponse("Movie not found.");

            existingMovie.Title = movie.Title;

            try
            {
                _movieRepository.Update(existingMovie);
                await _unitOfWork.CompleteAsync();

                return new MovieResponse(existingMovie);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new MovieResponse($"An error occurred when updating the movie: {ex.Message}");
            }
        }

        public async Task<MovieResponse> DeleteAsync(int id)
        {
            var existingMovie = await _movieRepository.FindByIdAsync(id);

            if (existingMovie == null)
                return new MovieResponse("Movie not found.");

            try
            {
                _movieRepository.Remove(existingMovie);
                await _unitOfWork.CompleteAsync();

                return new MovieResponse(existingMovie);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new MovieResponse($"An error occurred when deleting the movie: {ex.Message}");
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