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
        public Guid UserId { get; set; } // User who receives the notification

        /// <summary>
        /// The ID of the entity related to the notification (Card, Board, Column, etc.).
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Enum representing the type of entity (Card, Board, Column, etc.).
        /// </summary>
        public EntityType EntityType { get; set; }

        /// <summary>
        /// Notification message content.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Enum type to categorize the notification (Success, Warning, Info, Update, etc.).
        /// </summary>
        public NotificationType MessageType { get; set; }

        /// <summary>
        /// Indicates whether the notification has been read by the user.
        /// </summary>
        public bool IsRead { get; set; } = false;

        /// <summary>
        /// Indicates whether the notification has been sent.
        /// </summary>
        public bool IsSent { get; set; } = false;

        /// <summary>
        /// A short description of what entity property was changed (e.g., "Title Updated").
        /// </summary>
        public string? SpecificEntityChange { get; set; }

        /// <summary>
        /// Timestamp of when the notification was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
