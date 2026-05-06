using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Finlay.PharmaVigilance.Infrastructure;

public class CaptchaService : ICaptchaService
{
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;

    public CaptchaService(IConfiguration config, IHttpClientFactory httpClientFactory)
    {
        _config = config;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<bool> VerifyToken(string token)
    {
        try
        {
            var secret = _config["Recaptcha:SecretKey"];

            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.PostAsync(
                $"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={token}",
                null
            );

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<RecaptchaResponse>(json);

            return result?.success == true;
        }
        catch
        {
            return false;
        }


    }
}