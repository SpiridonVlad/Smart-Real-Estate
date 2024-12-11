using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Application.Email
{
    public class EmailService(IConfiguration configuration)
    {
        private readonly IConfiguration configuration = configuration;
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var emailSettings = configuration.GetSection("EmailSettings");
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(emailSettings["SenderEmail"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart("html")
            {
                Text = body
            };
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["SmtpPort"]), MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(emailSettings["SenderEmail"], emailSettings["SenderPassword"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
