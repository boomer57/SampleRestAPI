namespace SampleRestAPI.API.Domain.Models.Queries
{
    public class UsersQuery : Query
    {
        public int? MovieId { get; set; }

        public UsersQuery(int? movieId, int page, int itemsPerPage) : base(page, itemsPerPage)
        {
            MovieId = movieId;
        }
    }
}