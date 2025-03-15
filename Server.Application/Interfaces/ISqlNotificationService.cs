using Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Application.Interfaces
{
    public interface ISqlNotificationService
    {
        void CardTableDependency(string connectionString);
        void BoardTableDependency(string connectionString);
        void AttachmentTableDependency(string connectionString);
        void ColumnTableDependency(string connectionString);
        void NotificationTableDependency(string connectionString);
    }
}
