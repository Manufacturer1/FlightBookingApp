using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class UpdateAirlineDto
    {
        [Required]
        public int Id { get; set; }
        public int? BaggagePolicyId { get; set; }
        public string? Name { get; set; }
        public IFormFile? AirlineImage { get; set; }
        public string? AirlineBgColor { get; set; }
    }
}
