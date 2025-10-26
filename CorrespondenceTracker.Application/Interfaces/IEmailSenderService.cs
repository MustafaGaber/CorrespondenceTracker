
namespace CorrespondenceTracker.Application.Interfaces
{
    public interface IEmailSenderService
    {
        void SendEmail(string recipientEmail, string subject, string body);
    }
}