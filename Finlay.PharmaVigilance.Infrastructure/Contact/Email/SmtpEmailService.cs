// using MailKit.Net.Smtp;
// using MailKit.Security;
// using MimeKit;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.Logging;
// using Finlay.PharmaVigilance.Application.IServices;
// using Finlay.PharmaVigilance.Application.DTO;
// using Finlay.PharmaVigilance.Domain.Enum;

// namespace Finlay.PharmaVigilance.Infrastructure.Email;

// public class SmtpEmailService : IEmailService
// {
//     private readonly IConfiguration _configuration;
//     private readonly ILogger<SmtpEmailService> _logger;

//     public SmtpEmailService(IConfiguration configuration, ILogger<SmtpEmailService> logger)
//     {
//         _configuration = configuration;
//         _logger = logger;
//     }

//     public async Task SendEmailAsync<T>(
//         string toEmail,
//         EmailTemplateType templateType,
//         T templateData)
//     {
//         try
//         {
//             var host = _configuration["Email:Smtp:Host"];
//             var port = int.Parse(_configuration["Email:Smtp:Port"] ?? "587");
//             var user = _configuration["Email:Smtp:User"];
//             var password = _configuration["Email:Smtp:Password"];
//             var fromName = _configuration["Email:Smtp:FromName"];

//             // Validate configuration
//             if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
//             {
//                 _logger.LogError("SMTP configuration is incomplete. Check appsettings.json Email:Smtp settings.");
//                 throw new InvalidOperationException("SMTP configuration is incomplete.");
//             }

//             var subject = GetSubject(templateType);

//             var htmlBody = BuildHtml(templateType, templateData);

//             var message_obj = new MimeMessage();
//             message_obj.From.Add(new MailboxAddress(fromName, user));
//             message_obj.To.Add(new MailboxAddress("", toEmail));
//             message_obj.Subject = subject;

//             var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody };
//             message_obj.Body = bodyBuilder.ToMessageBody();

//             using (var client = new SmtpClient())
//             {
//                 await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
//                 await client.AuthenticateAsync(user, password);
//                 await client.SendAsync(message_obj);
//                 await client.DisconnectAsync(true);

//                 _logger.LogInformation($"Email enviado exitosamente a {toEmail}");
//             }
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError($"Error al enviar email a {toEmail}: {ex.Message}\n{ex.InnerException?.Message}");
//             throw;
//         }
//     }


//     private static string GetSubject(
//         EmailTemplateType templateType)
//     {
//         return templateType switch
//         {
//             EmailTemplateType.ActivateAccount =>
//                 "Activación de cuenta",

//             EmailTemplateType.AssignmentExpired =>
//                 "Asignación expirada",

//             _ => throw new ArgumentOutOfRangeException(
//                 nameof(templateType),
//                 templateType,
//                 null)
//         };
//     }

//     private static string BuildHtml<T>(
//         EmailTemplateType templateType,
//         T templateData)
//     {
//         return templateType switch
//         {
//             EmailTemplateType.ActivateAccount =>
//                 BuildActivateAccountHtml(
//                     templateData as ActivateAccountTemplate
//                     ?? throw new InvalidOperationException()),

//             EmailTemplateType.AssignmentExpired =>
//                 BuildAssignmentExpiredHtml(
//                     templateData as AssignmentExpiredTemplate
//                     ?? throw new InvalidOperationException()),

//             _ => throw new ArgumentOutOfRangeException(
//                 nameof(templateType),
//                 templateType,
//                 null)
//         };
//     }

//     private static string BuildActivateAccountHtml(
//         ActivateAccountTemplate data)
//     {
//         return $"""
//             <div style="font-family: Arial">

//                 <h1>Activación de cuenta</h1>

//                 <p>
//                     Hola {data.FullName}
//                 </p>

//                 <p>
//                     Haz click en el siguiente enlace:
//                 </p>

//                 <a href="{data.ActivationUrl}">
//                     Activar cuenta
//                 </a>

//             </div>
//             """;
//     }

//     private static string BuildAssignmentExpiredHtml(
//         AssignmentExpiredTemplate data)
//     {
//         return $"""
//             <div style="font-family: Arial">

//                 <h1>Asignación expirada</h1>

//                 <p>
//                     El reporte {data.ReportId}
//                     ha expirado y fue reabierto.
//                 </p>

//             </div>
//             """;
//     }
// }
