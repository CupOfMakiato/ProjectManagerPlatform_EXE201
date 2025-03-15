using Server.Application;
using Server.Application.Interfaces;
using Server.Application.Repositories;
using Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Application.Services
{
    public class NotificationPersonalService : INotificationPersonalService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public NotificationPersonalService(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddNotificationToDatabase(Notification notification)
        {
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangeAsync();
        }

        //public async Task<Notification> PrNotification(string message, string card, string EntityChange)
        //{
        //    //Get user who recieve notification
        //    var user = await _unitOfWork.userRepository.GetAllAsync();

        //    // Create Notification
        //    Notification notification = new Notification()
        //    {
        //        Message = message,
        //        MessageType = (Domain.Enums.NotificationType)3,
        //        IsRead = false,
        //        IsSent = false,
        //        CreationDate = DateTime.Now,
        //        SpecificEntityChange = EntityChange
        //    };

        //    foreach (var item in user)
        //    {
        //        notification.NotificationCreatedByUser = item.Id;

        //        await _notificationRepository.AddAsync(notification);
        //    }

        //    await _unitOfWork.SaveChangeAsync();

        //    return notification;
        //}

        public async Task<List<Notification>> GetUserNotifications(Guid userId)
        {
            return await _notificationRepository.GetUnreadNotificationsAsync(userId);
        }

        public async Task MarkNotificationAsRead(Guid notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true; // Mark as read
                _notificationRepository.Update(notification);
                await _unitOfWork.SaveChangeAsync();
            }
        }
    }
}
