using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Server.Application.Interfaces;
using Server.Application.Repositories;
using Server.Application.Services;
using Server.Domain.Entities;
using Server.Domain.Enums;
using Server.Infrastructure.Hubs;
using System;
using System.Threading.Tasks;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;
using TableDependency.SqlClient.Base.EventArgs;

namespace Server.Infrastructure.Services
{
    //public class SqlNotificationService : ISqlNotificationService, IDisposable
    //{
    //    private readonly IServiceScopeFactory _scopeFactory;

    //    private SqlTableDependency<Card> _cardTableDependency;
    //    private SqlTableDependency<Board> _boardTableDependency;
    //    private SqlTableDependency<Attachment> _attachmentTableDependency;
    //    private SqlTableDependency<Column> _columnTableDependency;
    //    private SqlTableDependency<Notification> _notificationTableDependency;

    //    private readonly IHubContext<NotificationHub> _notificationHub;

    //    public SqlNotificationService(IServiceScopeFactory scopeFactory, IHubContext<NotificationHub> hubContext)
    //    {
    //        _scopeFactory = scopeFactory;
    //        _notificationHub = hubContext;
    //    }

    //    public void CardTableDependency(string connectionString)
    //    {
    //        _cardTableDependency = new SqlTableDependency<Card>(connectionString, "Cards");
    //        _cardTableDependency.OnChanged += TableDependency_OnChanged;
    //        _cardTableDependency.OnError += TableDependency_OnError;
    //        _cardTableDependency.Start();
    //    }

    //    public void BoardTableDependency(string connectionString)
    //    {
    //        _boardTableDependency = new SqlTableDependency<Board>(connectionString, "Boards");
    //        _boardTableDependency.OnChanged += TableDependency_OnChanged;
    //        _boardTableDependency.OnError += TableDependency_OnError;
    //        _boardTableDependency.Start();
    //    }

    //    public void AttachmentTableDependency(string connectionString)
    //    {
    //        _attachmentTableDependency = new SqlTableDependency<Attachment>(connectionString, "Attachments");
    //        _attachmentTableDependency.OnChanged += TableDependency_OnChanged;
    //        _attachmentTableDependency.OnError += TableDependency_OnError;
    //        _attachmentTableDependency.Start();
    //    }

    //    public void ColumnTableDependency(string connectionString)
    //    {
    //        _columnTableDependency = new SqlTableDependency<Column>(connectionString, "Columns");
    //        _columnTableDependency.OnChanged += TableDependency_OnChanged;
    //        _columnTableDependency.OnError += TableDependency_OnError;
    //        _columnTableDependency.Start();
    //    }

    //    public void NotificationTableDependency(string connectionString)
    //    {
    //        _notificationTableDependency = new SqlTableDependency<Notification>(connectionString);
    //        _notificationTableDependency.OnChanged += TableDependency_OnChanged;
    //        _notificationTableDependency.OnError += TableDependency_OnError;
    //        _notificationTableDependency.Start();
    //    }

    //    private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
    //    {
    //        Console.WriteLine("ERROR in SqlTableDependency!");
    //        Console.WriteLine($"Error Message: {e.Error.Message}");

    //        if (e.Error.InnerException != null)
    //        {
    //            Console.WriteLine($"Inner Exception: {e.Error.InnerException.Message}");
    //        }

    //        Console.WriteLine($"Stack Trace: {e.Error.StackTrace}");
    //    }

    //    private async void TableDependency_OnChanged(object sender, RecordChangedEventArgs<Card> e)
    //    {
    //        if (e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.None)
    //            return;

    //        var card = e.Entity;
    //        await SendUpdateNotification(card, $"Card '{card.Title}' was updated.", "Card");
    //    }

    //    private async void TableDependency_OnChanged(object sender, RecordChangedEventArgs<Board> e)
    //    {
    //        if (e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.None)
    //            return;

    //        var board = e.Entity;
    //        await SendUpdateNotification(board, $"Board '{board.Title}' was updated.", "Board");
    //    }

    //    private async void TableDependency_OnChanged(object sender, RecordChangedEventArgs<Column> e)
    //    {
    //        if (e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.None)
    //            return;

    //        var column = e.Entity;
    //        await SendUpdateNotification(column, $"Column '{column.Title}' was updated.", "Column");
    //    }

    //    private async void TableDependency_OnChanged(object sender, RecordChangedEventArgs<Attachment> e)
    //    {
    //        if (e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.None)
    //            return;

    //        var attachment = e.Entity;
    //        await SendUpdateNotification(attachment, $"Attachment '{attachment.FileName}' was updated.", "Attachment");
    //    }

    //    private async void TableDependency_OnChanged(object sender, RecordChangedEventArgs<Notification> e)
    //    {
    //        if (e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.None)
    //            return;

    //        var notification = e.Entity;

    //        var userIdString = notification.NotificationCreatedByUser?.Id.ToString();
    //        if (!string.IsNullOrEmpty(userIdString))
    //        {
    //            await _notificationHub.Clients.User(userIdString).SendAsync("ReceiveNotification", notification.Message);
    //        }
    //    }

    //    private async Task SendUpdateNotification<TEntity>(TEntity entity, string message, string entityType) where TEntity : class
    //    {
    //        using (var scope = _scopeFactory.CreateScope())
    //        {
    //            var notificationPersonalService = scope.ServiceProvider.GetRequiredService<INotificationPersonalService>();
    //            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

    //            if (entity is BaseEntity baseEntity && baseEntity.CreatedBy.HasValue)
    //            {
    //                var userId = baseEntity.CreatedBy.Value; // not null
    //                var user = await userRepository.GetByIdAsync(userId);

    //                if (user == null)
    //                {
    //                    // Log warning if user is not found
    //                    Console.WriteLine($"Warning: User with ID {userId} not found.");
    //                    return;
    //                }

    //                var entityTypeEnum = Enum.TryParse(entityType, out EntityType parsedEntityType) ? parsedEntityType : EntityType.Unknown;

    //                var notification = new Notification
    //                {
    //                    EntityId = baseEntity.Id,
    //                    EntityType = entityTypeEnum,
    //                    Message = message,
    //                    MessageType = NotificationType.Update,
    //                    IsRead = false,
    //                    IsSent = true,
    //                    CreatedAt = DateTime.UtcNow,
    //                    SpecificEntityChange = $"{entityType} Update",
    //                    NotificationCreatedByUser = user
    //                };

    //                await notificationPersonalService.AddNotificationToDatabase(notification);

    //                var userIdString = userId.ToString();
    //                if (!string.IsNullOrEmpty(userIdString))
    //                {
    //                    await _notificationHub.Clients.User(userIdString).SendAsync("ReceiveNotification", message);
    //                }
    //            }
    //        }
    //    }

    //    public void Dispose()
    //    {
    //        _cardTableDependency?.Dispose();
    //        _boardTableDependency?.Dispose();
    //        _attachmentTableDependency?.Dispose();
    //        _columnTableDependency?.Dispose();
    //        _notificationTableDependency?.Dispose();
    //    }
    //}
}
