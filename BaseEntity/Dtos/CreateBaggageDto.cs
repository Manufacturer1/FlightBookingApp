using BaseEntity.Entities;
using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class CreateBaggageDto
    {
        [Required]
        public int FreeCheckedBags { get; set; } // Numarul bagajelor de cala gratuit
        [Required]
        public decimal CheckedWeightLimitKg { get; set; } // Limita in kg a bagajelor de cala gratuit
        [Required]
        public decimal ExtraCheckedBagFee { get; set; } // Taxa pentru extra bagaje
        [Required]
        public decimal OverWeightFeePerKg { get; set; } // Taxe pentru excess de greutate
        [Required]

        public int FreeCabinBags { get; set; } // Numarul de bagaje in cabina gratuite
        [Required]
        public decimal CabinWeightLimitKg { get; set; } // Limita in kg a fiecarui bagaj de cabina
        [Required]
        public decimal ExtraCabinBagFee { get; set; } // Taxa pentru extra bagaj de cabina
    }
}
