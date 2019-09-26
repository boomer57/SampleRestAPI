namespace SampleRestAPI.API.Domain.Models.Queries
{
    public class RatingsQuery : Query
    {
        public int? MovieId { get; set; }

        public RatingsQuery(int? movieId, int page, int itemsPerPage) : base(page, itemsPerPage)
        {
            MovieId = movieId;
        }
    }
}