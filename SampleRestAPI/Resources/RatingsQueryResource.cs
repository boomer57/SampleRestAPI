namespace SampleRestAPI.API.Resources
{
    public class RatingsQueryResource : QueryResource
    {
        public int? MovieId { get; set; }
        public int? UserId { get; set; }
    }
}