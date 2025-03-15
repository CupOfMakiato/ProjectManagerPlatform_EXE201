using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain.Entities
{
    public class Notification : BaseEntity
    {

        public Guid EntityId { get; set; }

        public EntityType EntityType { get; set; }

        public string Message { get; set; }

        public NotificationType MessageType { get; set; }

        public bool IsRead { get; set; } = false;

        public bool IsSent { get; set; } = false;

        public string? SpecificEntityChange { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public User NotificationCreatedByUser { get; set; }
    }
}
