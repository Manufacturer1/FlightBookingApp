using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class BaggageInputDto
    {
        [Required]
        [Range(0, 12)]
        public int CabinBags { get; set; }
        [Required]
        [Range(0, 100)]
        public decimal CabinTotalWeightKg { get; set; }
        [Required]
        [Range(0,15)]
        public int CheckedBags { get; set; }
        [Required]
        [Range(0,500)]
        public decimal CheckedTotalWeightKg { get; set; }


    }
}
