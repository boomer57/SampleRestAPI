using System.ComponentModel.DataAnnotations;

namespace SampleRestAPI.API.Resources
{
    public class SaveUserResource
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(0, 100)]
        public short QuantityInPackage { get; set; }

        [Required]
        [Range(1, 5)]
        public int UnitOfMeasurement { get; set; } // AutoMapper is going to cast it to the respective enum value
        
        [Required]
        public int MovieId { get; set; }
    }
}