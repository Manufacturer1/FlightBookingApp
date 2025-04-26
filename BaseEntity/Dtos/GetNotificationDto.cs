using BaseEntity.Entities;
using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class GetNotificationDto
    {
        public int Id { get; set; }
        public int PassengerId { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
    }
}
