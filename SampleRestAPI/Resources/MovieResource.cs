namespace SampleRestAPI.API.Resources
{
    public class MovieResource
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int YearReleased { get; set; }
        public int RunningTime { get; set; }
        public string Genres { get; set; }
        public double AvgRating { 
            get 
            {
                return 0;
            }
        }
    }
}
