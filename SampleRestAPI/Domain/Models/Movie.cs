using System.Collections.Generic;

namespace SampleRestAPI.API.Domain.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int YearReleased { get; set; }
        public int RunningTime { get; set; }
        public string Genres { get; set; }
        public double AvgRating
        {
            get
            {
                return 0;
            }
        }

        public IEnumerable<User> Users { get; internal set; }
    }
}