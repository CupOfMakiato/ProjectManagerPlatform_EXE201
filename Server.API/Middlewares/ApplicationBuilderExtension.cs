using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Server.Application.Interfaces;
using System;

namespace Server.API.Middlewares
{
    public static class ApplicationBuilderExtension
    {
        public static void UseSqlTableDependency(this IApplicationBuilder app, string connectionString)
        {
            Console.WriteLine("UseSqlTableDependency called...");

            // Resolve the service by its interface
            var service = app.ApplicationServices.GetService<ISqlNotificationService>();

            if (service == null)
            {
                Console.WriteLine("ERROR: SqlNotificationService is NULL.");
                return;
            }

            Console.WriteLine("SqlNotificationService retrieved successfully.");

            service.CardTableDependency(connectionString);
            service.BoardTableDependency(connectionString);
            service.AttachmentTableDependency(connectionString);
            service.ColumnTableDependency(connectionString);
            service.NotificationTableDependency(connectionString);
        }
    }
}
