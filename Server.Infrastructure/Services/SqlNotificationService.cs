using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Server.Application.Interfaces;
using Server.Application.Services;
using Server.Domain.Entities;
using Server.Infrastructure.Hubs;
using System;
using System.Threading.Tasks;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;

namespace Server.Infrastructure.Services
{
    public class SqlNotificationService : ISqlNotificationService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        NotificationHub _notificationHub;

        private SqlTableDependency<Card> _cardTableDependency;
        private SqlTableDependency<Board> _boardTableDependency;
        private SqlTableDependency<Attachment> _attachmentTableDependency;
        private SqlTableDependency<Column> _columnTableDependency;
        private SqlTableDependency<Notification> _notificationTableDependency;

        public SqlNotificationService(IServiceScopeFactory scopeFactory, NotificationHub hubContext)
        {
            _scopeFactory = scopeFactory;
            _notificationHub = hubContext;
        }

        public void CardTableDependency(string connectionString)
        {
            Console.WriteLine("Initializing CardTableDependency...");
            _cardTableDependency = new SqlTableDependency<Card>(connectionString, "Cards");
            _cardTableDependency.OnChanged += TableDependency_OnChanged;
            _cardTableDependency.OnError += TableDependency_OnError;
            _cardTableDependency.Start();
            Console.WriteLine($"CardTableDependency Status: {_cardTableDependency.Status}");
        }

        //public void BoardTableDependency(string connectionString)
        //{
        //    _boardTableDependency = new SqlTableDependency<Board>(connectionString, "Boards");
        //    _boardTableDependency.OnChanged += (sender, e) => TableDependency_OnChanged(e, "Board");
        //    _boardTableDependency.OnError += TableDependency_OnError;
        //    _boardTableDependency.Start();
        //}

        //public void AttachmentTableDependency(string connectionString)
        //{
        //    _attachmentTableDependency = new SqlTableDependency<Attachment>(connectionString, "Attachments");
        //    _attachmentTableDependency.OnChanged += (sender, e) => TableDependency_OnChanged(e, "Attachment");
        //    _attachmentTableDependency.OnError += TableDependency_OnError;
        //    _attachmentTableDependency.Start();
        //}

        //public void ColumnTableDependency(string connectionString)
        //{
        //    _columnTableDependency = new SqlTableDependency<Column>(connectionString, "Columns");
        //    _columnTableDependency.OnChanged += (sender, e) => TableDependency_OnChanged(e, "Column");
        //    _columnTableDependency.OnError += TableDependency_OnError;
        //    _columnTableDependency.Start();
        //}

        public void NotificationTableDependency(string connectionString)
        {
            Console.WriteLine("Initializing NotificationTableDependency...");
            _notificationTableDependency = new SqlTableDependency<Notification>(connectionString);
            _notificationTableDependency.OnChanged += TableDependency_OnChanged;
            _notificationTableDependency.OnError += TableDependency_OnError;
            _notificationTableDependency.Start();
            Console.WriteLine($"NotificationTableDependency: {_notificationTableDependency.Status}");
        }
        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(Notification)} SqlTableDependency error: {e.Error.Message}");
        }

        private async void TableDependency_OnChanged(object sender, RecordChangedEventArgs<Notification> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                var notification = e.Entity;
            }
        }

        //private async void TableDependency_OnChanged(object sender, RecordChangedEventArgs<Card> e)
        //{
        //    if (e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.None)
        //        return;

        //    using (var scope = _scopeFactory.CreateScope()) // Create a new DI scope
        //    {
        //        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationPersonalService>();

        //        var card = e.Entity;

        //        if (e.EntityOldValues == null) // Check if this is a new entity (Insert)
        //        {
        //            var notification = await notificationService.PrNotification($"New Card Added. {card.Title}", card.Title, "Card");
        //            await _notificationHub.SendNotificationToAll(notification);
        //        }
        //    }
        //}
        private async void TableDependency_OnChanged(object sender, RecordChangedEventArgs<Card> e)
        {
            // test
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                var card = e.Entity;
            }
        }

        public void Dispose()
        {
            _cardTableDependency?.Dispose();
            _boardTableDependency?.Dispose();
            _attachmentTableDependency?.Dispose();
            _columnTableDependency?.Dispose();
            _notificationTableDependency?.Dispose();
        }
    }
}
