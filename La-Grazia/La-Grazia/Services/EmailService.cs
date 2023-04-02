using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace La_Grazia.Services
{
    public interface IEmailService
    {
        void Send(string to, string subject, string html);
    }

    public class EmailService : IEmailService
    {
        public void Send(string to, string subject, string html)
        {
            // creating message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("lagrazia@yandex.com"));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // sending email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.yandex.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("lagrazia@yandex.com", "123321");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}