using CorrespondenceTracker.Application.Interfaces;
using System.Net;
using System.Net.Mail;

namespace CorrespondenceTracker.Infrastructure.EmailSender
{
    public class EmailSenderService : IEmailSenderService
    {
        private const string GMAIL_HOST = "smtp.gmail.com";
        private const int GMAIL_PORT = 587;
        private const string senderEmail = "nucadeveloping@gmail.com";
        private const string senderAppPassword = "nlne crlt bntd yank";
        /// <summary>
        /// Attempts to send an email using the provided credentials.
        /// </summary>
        /// <param name="senderEmail">The full Gmail address of the sender.</param>
        /// <param name="senderAppPassword">The 16-character Google App Password.</param>
        /// <param name="recipientEmail">The email address of the recipient.</param>
        /// <param name="subject">The subject line of the email.</param>
        /// <param name="body">The plain-text body of the email.</param>
        public void SendEmail(string recipientEmail, string subject, string body)
        {
            if (string.IsNullOrEmpty(senderEmail) || string.IsNullOrEmpty(senderAppPassword))
            {
                throw new ArgumentException("Sender email and App Password must be provided.");
            }

            // Create the MailMessage object
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(senderEmail);
            mail.To.Add(recipientEmail);
            mail.Subject = subject;
            mail.Body = body;

            // Create the SmtpClient object for Gmail
            SmtpClient smtpServer = new SmtpClient(GMAIL_HOST);
            smtpServer.Port = GMAIL_PORT;
            smtpServer.EnableSsl = true; // Enable SSL
            smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpServer.UseDefaultCredentials = false;

            // Set the credentials using your email and the App Password
            smtpServer.Credentials = new NetworkCredential(senderEmail, senderAppPassword);

            // Send the email
            // This method will throw an SmtpException if sending fails
            smtpServer.Send(mail);
        }
    }
}
