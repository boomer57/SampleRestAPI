namespace SampleRestAPI.API.Resources
{
    public class UserResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int QuantityInPackage { get; set; }
        public string UnitOfMeasurement { get; set; }
        public MovieResource Movie {get;set;}
    }
}