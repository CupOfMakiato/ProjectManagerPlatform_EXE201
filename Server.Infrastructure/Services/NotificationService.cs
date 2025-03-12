using Microsoft.AspNetCore.SignalR;
using Server.Application;
using Server.Application.Interfaces;
using Server.Application.Repositories;
using Server.Contracts.DTO.User;
using Server.Domain.Entities;
using Server.Domain.Enums;
using Server.Infrastructure.Hubs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ICardRepository _cardRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(INotificationRepository notificationRepository,
                                   IUserRepository userRepository, ICardRepository cardRepository,
                                   IEmailService emailService, IUnitOfWork unitOfWork,
                                   IHubContext<NotificationHub> hubContext)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _cardRepository = cardRepository;
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Sends a notification to a user and persists it in the database.
        /// </summary>
        public async Task SendNotificationAsync(Notification notification)
        {
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));

            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangeAsync();

            await NotifyUserAsync(notification);
        }

        /// <summary>
        /// Sends a custom notification to a specific user.
        /// </summary>
        public async Task SendNotificationToUserAsync(Guid userId, string message, NotificationType type, EntityType entityType, Guid entityId)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                MessageType = type,
                EntityType = entityType,
                EntityId = entityId,
                CreatedAt = DateTime.UtcNow,
                IsSent = true
            };

            await SendNotificationAsync(notification);
        }

        /// <summary>
        /// Sends multiple notifications at once.
        /// </summary>
        public async Task SendBulkNotificationsAsync(List<Notification> notifications)
        {
            if (notifications == null || notifications.Count == 0)
                throw new ArgumentException("Notifications list cannot be empty.", nameof(notifications));

            await _notificationRepository.AddRangeAsync(notifications);
            await _unitOfWork.SaveChangeAsync();

            foreach (var notification in notifications)
            {
                await NotifyUserAsync(notification);
            }
        }

        /// <summary>
        /// Retrieves notifications for a specific user.
        /// </summary>
        public async Task<List<Notification>> GetUserNotificationsAsync(Guid userId)
        {
            return await _notificationRepository.GetUnreadNotificationsAsync(userId);
        }

        /// <summary>
        /// Marks a notification as read.
        /// </summary>
        public async Task MarkAsReadAsync(Guid notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null)
                throw new KeyNotFoundException($"Notification with ID {notificationId} not found.");

            notification.IsRead = true;
            _notificationRepository.Update(notification);
            await _unitOfWork.SaveChangeAsync();
        }

        /// <summary>
        /// Notifies a user in real-time via SignalR if they are connected.
        /// </summary>
        private async Task NotifyUserAsync(Notification notification)
        {
            if (NotificationHub._ConnectionsMap.TryGetValue(notification.UserId, out var connectionId))
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("ReceivedNotification", notification);
            }
        }

        /// <summary>
        /// Sends a due date reminder notification via SignalR and Email.
        /// </summary>
        public async Task SendDueDateReminderAsync(Card card)
        {
            var userId = card.CardCreatedByUser.Id; 
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return;

            string message = $"Reminder: Your task '{card.Title}' is due on {card.DueDate?.ToString("yyyy-MM-dd HH:mm")}";

            // Save the notification
            var notification = new Notification
            {
                UserId = user.Id,
                Message = message,
                MessageType = NotificationType.Warning, // Changed from Reminder to Warning
                EntityType = EntityType.Card,
                EntityId = card.Id,
                CreatedAt = DateTime.UtcNow,
                IsSent = true
            };

            await SendNotificationAsync(notification);

            // Send Email Notification
            var emailDto = new EmailDTO
            {
                To = user.Email,
                Subject = "Task Due Date Reminder",
                Body = $@"
                <html>
                    <body style='font-family: Arial, sans-serif;'>
                        <h3>Task Due Date Reminder</h3>
                        <p>Hello {user.UserName},</p>
                        <p>Your task <strong>{card.Title}</strong> is due on <strong>{card.DueDate?.ToString("yyyy-MM-dd HH:mm")}</strong>.</p>
                        <p>Don't forget to complete it on time!</p>
                        <p>Best regards,<br/>ProManager!</p>
                    </body>
                </html>"
            };

            await _emailService.SendEmailAsync(emailDto);
        }



    }
}
