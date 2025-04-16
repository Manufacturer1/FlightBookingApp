namespace BaseEntity.Entities
{
    public class Airport
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;

        public virtual ICollection<Flight>? DepartingFlights { get; set; }
        public virtual ICollection<Flight>? ArrivalFlights { get; set; }

    }
}
