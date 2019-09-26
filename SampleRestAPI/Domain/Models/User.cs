namespace SampleRestAPI.API.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public short QuantityInPackage { get; set; }
        public EUnitOfMeasurement UnitOfMeasurement { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

    }
}