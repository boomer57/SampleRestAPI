using System.ComponentModel.DataAnnotations;

namespace SampleRestAPI.API.Resources
{
    public class SaveMovieResource
    {
        [Required]
        [MaxLength(30)]
        public string Title { get; set; }
    }
}