using BaseEntity.Entities;

namespace BaseEntity.Dtos
{
    public class CreateBaggageDto
    {
        [Rquired]
        public int FreeCheckedBags { get; set; } // Numarul bagajelor de cala gratuit
        [Rquired]
        public decimal CheckedWeightLimitKg { get; set; } // Limita in kg a bagajelor de cala gratuit
        [Rquired]
        public decimal ExtraCheckedBagFee { get; set; } // Taxa pentru extra bagaje
        [Rquired]
        public decimal OverWeightFeePerKg { get; set; } // Taxe pentru excess de greutate
        [Rquired]

        public int FreeCabinBags { get; set; } // Numarul de bagaje in cabina gratuite
        [Rquired]
        public decimal CabinWeightLimitKg { get; set; } // Limita in kg a fiecarui bagaj de cabina
        [Rquired]
        public decimal ExtraCabinBagFee { get; set; } // Taxa pentru extra bagaj de cabina
    }
}
