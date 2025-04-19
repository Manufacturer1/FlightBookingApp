namespace BaseEntity.Dtos
{
    public class UpdateDiscountDto
    {
        public string? Name { get; set; } 
        public decimal? Percentage { get; set; }
        public bool? IsActive { get; set; }
    }
}
