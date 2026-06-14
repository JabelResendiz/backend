using System.Text;
using System.Text.Json;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Domain.Enum;
using Finlay.PharmaVigilance.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Finlay.PharmaVigilance.Infrastructure.Email;

public class WhatsAppService : IMessageService
{
    // private readonly IHttpClientFactory _httpClientFactory;

    private readonly HttpClient _client;
    private readonly WhatsAppSettings _settings;
    private readonly ILogger<WhatsAppService> _logger;


    public WhatsAppService(
        HttpClient client,
        IOptions<WhatsAppSettings> options,
        ILogger<WhatsAppService> logger)
    {
        _client = client;
        _settings = options.Value;
        _logger = logger;

        _client.DefaultRequestHeaders.Add("X-API-Key", _settings.ApiKey);
        _client.Timeout = TimeSpan.FromSeconds(_settings.TimeoutSeconds);
    }

    public async Task SendEmailAsync<T>(
        string phoneNumber,
        EmailTemplateType templateType,
        T templateData) where T : IBasicTemplate
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_settings.SessionId))
            {
                _logger.LogWarning("SendMessageAsync: sessionId no puede estar vacío");
                return;
            }

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                _logger.LogWarning("SendMessageAsync: phoneNumber no puede estar vacío");
                return;
            }

            Console.WriteLine($"{_settings.ApiBaseUrl} ----> {_settings.SessionId}");


            string messageText;
            try
            {
                messageText = WhatsAppMessageFactory.Build(templateData);
            }
            catch (NotSupportedException ex)
            {
                _logger.LogWarning(ex, $"Evento {typeof(T).Name} no soportado para WhatsApp");
                return;
            }


            Console.WriteLine(messageText);

            // Normalizar número de teléfono - remover caracteres especiales
            var cleanPhoneNumber = System.Text.RegularExpressions.Regex.Replace(phoneNumber, @"[^\d]", "");

            // Formato correcto para OpenWA
            var chatId = $"{cleanPhoneNumber}@c.us";

            Console.WriteLine(chatId);


            var requestBody = new
            {
                chatId = chatId,
                text = messageText
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var endpoint = $"{_settings.ApiBaseUrl}/sessions/{_settings.SessionId}/messages/send-text";

            var response = await _client.PostAsync(endpoint, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "WhatsApp enviado. Status: {StatusCode}. Response: {Response}",
                    response.StatusCode,
                    responseContent);
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"❌ Error al enviar mensaje WhatsApp: {response.StatusCode} - {errorContent}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al enviar confirmación de creación de reporte por WhatsApp");
            return;
        }
    }

}