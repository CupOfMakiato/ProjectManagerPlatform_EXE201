using Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Interfaces
{
    public interface INotificationPersonalService
    {
        Task AddNotificationToDatabase(Notification notification);
        Task<List<Notification>> GetUserNotifications(Guid userId);
        Task MarkNotificationAsRead(Guid notificationId);
        //Task<Notification> PrNotification(string message, string card, string EntityChange);
    }
}
