using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Entities
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public int PassengerId { get; set; }
        [StringLength(500)]
        public string Subject { get; set; } = string.Empty;
        [StringLength(1000)]
        public string Body { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
        public Passenger? Passenger { get; set; }
    }
}
