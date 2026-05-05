using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Finlay.PharmaVigilance.Application.IServices;

namespace Finlay.PharmaVigilance.Infrastructure.Email;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(IConfiguration configuration, ILogger<SmtpEmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        try
        {
            var host = _configuration["Email:Smtp:Host"];
            var port = int.Parse(_configuration["Email:Smtp:Port"] ?? "587");
            var user = _configuration["Email:Smtp:User"];
            var password = _configuration["Email:Smtp:Password"];
            var fromName = _configuration["Email:Smtp:FromName"];

            // Validate configuration
            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
            {
                _logger.LogError("SMTP configuration is incomplete. Check appsettings.json Email:Smtp settings.");
                throw new InvalidOperationException("SMTP configuration is incomplete.");
            }

            var message_obj = new MimeMessage();
            message_obj.From.Add(new MailboxAddress(fromName, user));
            message_obj.To.Add(new MailboxAddress("", toEmail));
            message_obj.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = message };
            message_obj.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(user, password);
                await client.SendAsync(message_obj);
                await client.DisconnectAsync(true);

                _logger.LogInformation($"Email enviado exitosamente a {toEmail}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al enviar email a {toEmail}: {ex.Message}\n{ex.InnerException?.Message}");
            throw;
        }
    }
}
