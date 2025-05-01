namespace BaseEntity.Entities
{
    public class Discount 
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Percentage { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
