using System.ComponentModel.DataAnnotations;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class MedicalReportDto : ReportDto
{
    [Required(ErrorMessage = "Causality is required.")]
    public CausalityLevel? Causality { get; set; }
    [Required(ErrorMessage = "Clinical Significance is required.")]
    public ClinicalSignificance? ClinicalSignificance { get; set; }
}