namespace SampleRestAPI.API.Domain.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public short RatingValue { get; set; }

    }
}