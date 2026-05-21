using System.ComponentModel.DataAnnotations;

namespace Finlay.PharmaVigilance.Application.DTO;

public class ReportAccessQueryDto
{
    [Required(ErrorMessage = "Notification Number is required.")]
    public string NotificationNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Captcha Token information is required.")]
    public string Token { get; set; } = string.Empty;
}