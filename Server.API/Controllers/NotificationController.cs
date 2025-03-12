using Microsoft.AspNetCore.Mvc;
using Server.Application.Interfaces;
using Server.Domain.Entities;
using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.API.Controllers
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Sends a notification to a user.
        /// </summary>
        [HttpPost("SendNotificationCustom")]
        public async Task<IActionResult> SendNotification(Guid userId, string message, NotificationType type, EntityType entityType, Guid entityId)
        {
            await _notificationService.SendNotificationToUserAsync(userId, message, type, entityType, entityId);
            return Ok(new { Message = "Notification sent successfully!" });
        }

        /// <summary>
        /// Retrieves notifications for a user.
        /// </summary>
        [HttpGet("GetUserNotifications{userId}")]
        public async Task<IActionResult> GetUserNotifications(Guid userId)
        {
            var notifications = await _notificationService.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }

        /// <summary>
        /// Marks a notification as read.
        /// </summary>
        [HttpPost("MarkAsRead/{notificationId}")]
        public async Task<IActionResult> MarkAsRead(Guid notificationId)
        {
            await _notificationService.MarkAsReadAsync(notificationId);
            return Ok(new { Message = "Notification marked as read!" });
        }
    }
}
