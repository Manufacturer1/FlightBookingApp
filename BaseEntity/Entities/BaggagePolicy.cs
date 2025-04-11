using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Entities
{
    public class BaggagePolicy
    {
        public int Id { get; set; }
        public int FreeCheckedBags { get; set; } // Numarul bagajelor de cala gratuit
        public decimal CheckedWeightLimitKg { get; set; } // Limita in kg a bagajelor de cala gratuit
        public decimal ExtraCheckedBagFee { get; set; } // Taxa pentru extra bagaje
        public decimal OverWeightFeePerKg { get; set; } // Taxe pentru excess de greutate

        public int FreeCabinBags { get; set; } // Numarul de bagaje in cabina gratuite
        public decimal CabinWeightLimitKg { get; set; } // Limita in kg a fiecarui bagaj de cabina
        public decimal ExtraCabinBagFee { get; set; } // Taxa pentru extra bagaj de cabina
        public ICollection<Airline>? Airlines { get; set; }
    }
}
