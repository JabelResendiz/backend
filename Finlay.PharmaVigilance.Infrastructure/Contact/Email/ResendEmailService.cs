// using System.Net.Http.Headers;
// using System.Text;
// using System.Text.Json;
// using Finlay.PharmaVigilance.Application.IServices;
// using Finlay.PharmaVigilance.Infrastructure.Settings;
// using Microsoft.Extensions.Options;

// public class ResendEmailService : IEmailService
// {
//     private readonly HttpClient _httpClient;
//     private readonly ResendSettings _settings;

//     public ResendEmailService(HttpClient httpClient, IOptions<ResendSettings> options)
//     {
//         _httpClient = httpClient;
//         _settings = options.Value;

//         _httpClient.DefaultRequestHeaders.Authorization =
//             new AuthenticationHeaderValue("Bearer", _settings.ApiKey);
//     }

//     public async Task SendEmailAsync(string toEmail, string subject, string message)
//     {
//         var payload = new
//         {
//             from = _settings.FromEmail,
//             to = toEmail,
//             subject = subject,
//             html = message
//         };

//         var json = JsonSerializer.Serialize(payload);

//         var response = await _httpClient.PostAsync(
//             "https://api.resend.com/emails",
//             new StringContent(json, Encoding.UTF8, "application/json")
//         );

//         response.EnsureSuccessStatusCode();
//     }
// }