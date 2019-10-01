using System.Collections.Generic;

namespace SampleRestAPI.API.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Movie Movie { get; internal set; }
    }
}