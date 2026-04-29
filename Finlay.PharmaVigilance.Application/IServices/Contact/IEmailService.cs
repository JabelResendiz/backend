namespace Finlay.PharmaVigilance.Application.IServices;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string message);
}