namespace Finlay.PharmaVigilance.Application.DTO;

public class RecaptchaResponse
{
    public bool success { get; set; }
    public string challenge_ts { get; set; } = string.Empty;
    public string hostname { get; set; } = string.Empty;
}