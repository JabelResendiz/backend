using System.ComponentModel.DataAnnotations;

namespace Finlay.PharmaVigilance.Application.DTO;


public class PublicAefiReportDto : ReportDto
{
    [Required(ErrorMessage = "Captcha Token information is required.")]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "Reporter information is required.")]
    public ReporterDto Reporter { get; set; } = null!;

}