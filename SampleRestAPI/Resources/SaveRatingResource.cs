using System.ComponentModel.DataAnnotations;

namespace SampleRestAPI.API.Resources
{
    public class SaveRatingResource
    {
        [Required]
        [Range(0, 5)]
        public int RatingValue { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}