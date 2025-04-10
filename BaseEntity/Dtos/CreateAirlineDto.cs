using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class CreateAirlineDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public IFormFile? AirlineImage { get; set; }
        [Required]
        public string AirlineBgColor { get; set; } = "#0D78C9FF";

        [Required]
        public int BaggagePolicyId { get; set; }
    }
}
