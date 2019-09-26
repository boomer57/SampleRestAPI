using System.Collections.Generic;

namespace SampleRestAPI.API.Domain.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int YearOfRelease { get; set; }
        public string Genres { get; set; }
        public IList<User> Users { get; set; } = new List<User>();
    }
}