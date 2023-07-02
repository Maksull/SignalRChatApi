using Core.Contracts.Services.EmailService;
using Infrastructure.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;


namespace Infrastructure.Services
{
    public sealed class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMessageAsync(EmailRequest emailRequest)
        {
            MimeMessage email = new();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailConfiguration:EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(emailRequest.To));
            email.Subject = emailRequest.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = emailRequest.Body };

            using SmtpClient smtp = new();
            await smtp.ConnectAsync(_configuration.GetSection("EmailConfiguration:EmailHost").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration.GetSection("EmailConfiguration:EmailUsername").Value, _configuration.GetSection("EmailConfiguration:EmailPassword").Value);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
