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
                await CheckDueDateRemindersAsync();
                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken); // Run every 10 minutes
            }
        }

        private async Task CheckDueDateRemindersAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var cardRepository = scope.ServiceProvider.GetRequiredService<ICardRepository>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

            var upcomingDueDate = DateTime.UtcNow.AddHours(6); // Notify tasks due in the next 6 hours
            var dueCards = await cardRepository.GetCardsDueBeforeAsync(upcomingDueDate);

            foreach (var card in dueCards)
            {
                await notificationService.SendDueDateReminderAsync(card);
            }

            _logger.LogInformation("Checked and sent due date reminders.");
        }
    }
}
