namespace Finlay.PharmaVigilance.Infrastructure.Settings;

public class WhatsAppSettings
{
    public string ApiBaseUrl { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
    public string SessionId { get; set; } = null!;
    public double TimeoutSeconds { get; set; }
}
