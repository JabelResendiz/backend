using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Domain.Enum;
using Finlay.PharmaVigilance.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Finlay.PharmaVigilance.Infrastructure.Email;

public class EmailJsService : IEmailService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly EmailJsSettings _settings;
    private readonly string _logoDataUri;

    public EmailJsService(
        IHttpClientFactory httpClientFactory,
        IOptions<EmailJsSettings> options)
    {
        _httpClientFactory = httpClientFactory;
        _settings = options.Value;
        _logoDataUri = GetLogoDataUri(); // Se carga una sola vez al instanciar
    }

    public async Task SendEmailAsync<T>(
        string toEmail,
        EmailTemplateType templateType,
        T templateData)
    {
        var url = "https://api.emailjs.com/api/v1.0/email/send";

        var templateId = GetTemplateId(templateType);
        var templateParams = ConvertToDictionary(templateData);
        templateParams["email"] = toEmail;
        templateParams["logo"] = _logoDataUri;

        var payload = new
        {
            service_id = _settings.ServiceId,
            template_id = templateId,
            user_id = _settings.UserId,
            accessToken = _settings.AccessToken,
            template_params = templateParams
        };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var client = _httpClientFactory.CreateClient();

        var response = await client.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error en EmailJS: {error}");
        }
    }

    private static string GetLogoDataUri()
    {
        string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo.png");

        if (!File.Exists(imagePath))
            throw new FileNotFoundException($"No se encontró logo.png en: {imagePath}");

        byte[] imageBytes = File.ReadAllBytes(imagePath);
        return $"data:image/png;base64,{Convert.ToBase64String(imageBytes)}";
    }


    private string GetTemplateId(
       EmailTemplateType templateType)
    {
        return templateType switch
        {
            EmailTemplateType.ActivateAccount =>
                _settings.ActivateAccount,

            EmailTemplateType.SelfReportConfirmation =>
                _settings.SelfReportConfirmation,

            EmailTemplateType.AssignmentExpired =>
                _settings.AssignmentExpired,

            EmailTemplateType.SectionReportAlert =>
                _settings.SectionReportAlert,

            EmailTemplateType.MedicalReviewerAssignment =>
                _settings.MedicalReviewerAssignment,

            _ => throw new ArgumentOutOfRangeException(
                nameof(templateType),
                $"Template type is not supported: {templateType}")
        };
    }


    private static Dictionary<string, string>
     ConvertToDictionary<T>(T templateData)
    {
        if (templateData == null)
        {
            return new Dictionary<string, string>();
        }

        return typeof(T)
            .GetProperties(
                BindingFlags.Public |
                BindingFlags.Instance)
            .ToDictionary(
                property =>
                    property
                        .GetCustomAttribute<JsonPropertyNameAttribute>()
                        ?.Name
                    ?? property.Name,

                property =>
                    property
                        .GetValue(templateData)?
                        .ToString()
                    ?? string.Empty
            );
    }
}