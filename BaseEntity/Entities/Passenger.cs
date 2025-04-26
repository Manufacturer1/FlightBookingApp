using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Entities
{
    public class Passenger
    {
        public int Id { get; set; }
        public int PassportIdentityId { get; set; }
        public int ContactDetailsId { get; set; }
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(100)]
        public string Surname { get; set; } = string.Empty;
        public DateTime BirthDay { get; set; }
        [StringLength(100)]
        public string Nationality { get; set; } = string.Empty;
        public PassportIdentity? PassportIdentity { get; set; }
        public ContactDetails? ContactDetails { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
    }
}
