using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Server.Application.Interfaces;
using Server.Application.Repositories;
using Server.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Infrastructure.Services
{
    public class DueDateReminderService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DueDateReminderService> _logger;

        public DueDateReminderService(IServiceScopeFactory scopeFactory, ILogger<DueDateReminderService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckDueDateReminders();
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Run every hour
            }
        }

        private async Task CheckDueDateReminders()
        {
            using var scope = _scopeFactory.CreateScope();
            var cardRepository = scope.ServiceProvider.GetRequiredService<ICardRepository>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

            var now = DateTime.UtcNow;

            // Fetch cards that have reminders set and are within the notification window
            var dueCards = await cardRepository.GetCardsWithUpcomingReminders(now);

            foreach (var card in dueCards)
            {
                await notificationService.SendDueDateReminder(card);
            }

            _logger.LogInformation("Checked and sent due date reminders.");
        }

    }
}
