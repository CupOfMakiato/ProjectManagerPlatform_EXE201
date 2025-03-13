using Server.Domain.Entities;
using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationAsync(Notification notification);
        Task SendNotificationToUserAsync(Guid userId, string message, NotificationType type, EntityType entityType, Guid entityId);
        Task SendBulkNotificationsAsync(List<Notification> notifications);
        Task<List<Notification>> GetUserNotificationsAsync(Guid userId);
        Task MarkAsReadAsync(Guid notificationId);
        // due date card reminder
        Task SendDueDateReminder(Card card);
    }
}
