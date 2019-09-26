namespace SampleRestAPI.API.Resources
{
    public class RatingResource
    {
        public int Id { get; set; }
        public int RatingValue { get; set; }
        public MovieResource Movie {get;set;}
        public UserResource User { get; set; }
    }
}