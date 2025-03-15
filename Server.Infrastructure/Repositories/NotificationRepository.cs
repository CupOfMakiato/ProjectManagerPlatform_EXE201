using Microsoft.EntityFrameworkCore;
using Server.Application.Interfaces;
using Server.Infrastructure.Data;
using Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1;
using Server.Application.Repositories;
using Server.Infrastructure.Repositories;

public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{
    public NotificationRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService)
        : base(context, timeService, claimsService)
    {
    }

    /// <summary>
    /// Fetch unread notifications for a specific user.
    /// </summary>
    public async Task<List<Notification>> GetUnreadNotificationsAsync(Guid userId)
    {
        return await _dbSet.Where(n => n.NotificationCreatedByUser.CreatedBy == userId && !n.IsRead)
                           .OrderByDescending(n => n.CreatedAt)
                           .ToListAsync();
    }
}
