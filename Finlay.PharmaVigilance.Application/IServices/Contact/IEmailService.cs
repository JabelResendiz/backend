using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IEmailService
{
    Task SendEmailAsync<T>(
        string toEmail,
        EmailTemplateType template,
        T templateData);
}