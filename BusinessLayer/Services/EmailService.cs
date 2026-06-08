using BusinessLayer.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using ModelLayer.DTOs;

namespace BusinessLayer.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(EmailDTO emailDTO)
        {
            var email = new MimeMessage();

            email.From.Add(
                new MailboxAddress(
                    "Fundoo Notes",
                    _configuration["SmtpSettings:SenderEmail"]));

            email.To.Add(
                MailboxAddress.Parse(emailDTO.ToEmail));

            email.Subject = emailDTO.Subject;

            email.Body = new TextPart("html")
            {
                Text = emailDTO.Body
            };

            using var smtp = new SmtpClient();

            smtp.Connect(
                _configuration["SmtpSettings:Host"],
                int.Parse(_configuration["SmtpSettings:Port"]),
                MailKit.Security.SecureSocketOptions.StartTls);

            smtp.Authenticate(
                _configuration["SmtpSettings:SenderEmail"],
                _configuration["SmtpSettings:Password"]);

            smtp.Send(email);

            smtp.Disconnect(true);
        }
    }
}