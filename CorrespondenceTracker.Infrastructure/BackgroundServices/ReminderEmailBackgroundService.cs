using CorrespondenceTracker.Application.Interfaces;
using CorrespondenceTracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CorrespondenceTracker.Infrastructure.BackgroundServices
{
    public class ReminderEmailBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ReminderEmailBackgroundService> _logger;

        public ReminderEmailBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<ReminderEmailBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Reminder Email Background Service is starting.");

            // Execute immediately on startup
            try
            {
                _logger.LogInformation("Executing initial reminder check on startup.");
                await ProcessRemindersAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during startup reminder check.");
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Calculate delay until the next hour
                    var now = DateTime.Now;
                    var nextHour = now.Date.AddHours(now.Hour + 1);
                    var delay = nextHour - now;

                    _logger.LogInformation(
                        "Next execution scheduled at {NextHour}. Waiting for {DelayMinutes} minutes.",
                        nextHour, delay.TotalMinutes);

                    // Wait until the next hour
                    await Task.Delay(delay, stoppingToken);

                    // Execute at the beginning of the hour
                    await ProcessRemindersAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    // Expected when the service is stopping
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing reminders.");

                    // If an error occurs, wait a short time before trying again
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }

            _logger.LogInformation("Reminder Email Background Service is stopping.");
        }

        private async Task ProcessRemindersAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<CorrespondenceDatabaseContext>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailSenderService>();

            var now = DateTime.Now;
            var windowStart = now.AddMinutes(-30);
            var windowEnd = now.AddMinutes(120);

            _logger.LogInformation(
                "Checking for reminders between {WindowStart} and {WindowEnd}",
                windowStart, windowEnd);

            // Find reminders that need email notification
            var remindersToProcess = await dbContext.Reminders
                .Include(r => r.Correspondence)
                    .ThenInclude(c => c.AssignedUser)
                .Where(r =>
                    r.SendEmailMessage &&
                    !r.IsEmailSent &&
                    !r.IsCompleted &&
                    !r.IsDismissed &&
                     r.RemindTime >= windowStart &&
                     r.RemindTime <= windowEnd)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Found {Count} reminders to process.", remindersToProcess.Count);

            foreach (var reminder in remindersToProcess)
            {
                try
                {
                    await SendReminderEmailAsync(reminder, emailService, cancellationToken);

                    // Mark email as sent
                    reminder.MarkEmailAsSent();
                    await dbContext.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation(
                        "Successfully sent reminder email for Reminder ID: {ReminderId}, Correspondence ID: {CorrespondenceId}",
                        reminder.Id, reminder.CorrespondenceId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Failed to send reminder email for Reminder ID: {ReminderId}",
                        reminder.Id);
                }
            }
        }

        private async Task SendReminderEmailAsync(
            Domain.Entities.Reminder reminder,
            IEmailSenderService emailService,
            CancellationToken cancellationToken)
        {
            var correspondence = reminder.Correspondence;
            var assignedUser = correspondence?.AssignedUser;

            if (assignedUser == null || string.IsNullOrWhiteSpace(assignedUser.Email))
            {
                _logger.LogWarning(
                    "Cannot send reminder email for Reminder ID: {ReminderId} - No assigned user or email address.",
                    reminder.Id);
                return;
            }

            // Arabic email subject
            string emailSubject = "تذكير بالرد على خطاب";

            // Build email body
            string emailBody = BuildEmailBody(reminder, correspondence);

            // Send email
            await Task.Run(() => emailService.SendEmail(
                assignedUser.Email,
                emailSubject,
                emailBody), cancellationToken);
        }

        private string BuildEmailBody(Domain.Entities.Reminder reminder, Domain.Entities.Correspondence? correspondence)
        {
            var body = $@"
                    <div dir='rtl' style='font-family: Arial, sans-serif;'>
                        <h2>تذكير بالرد على خطاب</h2>
                        <p><strong>وقت التذكير:</strong> {reminder.RemindTime:yyyy-MM-dd HH:mm}</p>
    
                        {(!string.IsNullOrWhiteSpace(reminder.Message) ? $"<p><strong>الرسالة:</strong> {reminder.Message}</p>" : "")}
    
                        {(correspondence != null ? $@"
                        <h3>تفاصيل الخطاب</h3>
                        <ul>
                            <li><strong>الاتجاه:</strong> {(correspondence.Direction == Domain.Entities.CorrespondenceDirection.Incoming ? "وارد" : "صادر")}</li>
                            <li><strong>الأولوية:</strong> {GetPriorityText(correspondence.PriorityLevel)}</li>
                            {(!string.IsNullOrWhiteSpace(correspondence.IncomingNumber) ? $"<li><strong>الرقم الوارد:</strong> {correspondence.IncomingNumber}</li>" : "")}
                            {(correspondence.IncomingDate.HasValue ? $"<li><strong>تاريخ الورود:</strong> {correspondence.IncomingDate.Value:yyyy-MM-dd}</li>" : "")}
                            {(!string.IsNullOrWhiteSpace(correspondence.Summary) ? $"<li><strong>الملخص:</strong> {correspondence.Summary}</li>" : "")}
                        </ul>
                        " : "")}
    
                        <hr />
                        <p style='color: #666; font-size: 12px;'>هذه رسالة تلقائية من نظام متابعة الخطابات </p>
                    </div>
                    ";
            return body;
        }

        private string GetPriorityText(Domain.Entities.PriorityLevel priority)
        {
            return priority switch
            {
                Domain.Entities.PriorityLevel.Critical => "حرجة",
                Domain.Entities.PriorityLevel.UrgentAndImportant => "عاجلة ومهمة",
                Domain.Entities.PriorityLevel.Urgent => "عاجلة",
                Domain.Entities.PriorityLevel.Important => "مهمة",
                Domain.Entities.PriorityLevel.Medium => "متوسطة",
                Domain.Entities.PriorityLevel.Low => "منخفضة",
                _ => "غير محدد"
            };
        }
    }
}