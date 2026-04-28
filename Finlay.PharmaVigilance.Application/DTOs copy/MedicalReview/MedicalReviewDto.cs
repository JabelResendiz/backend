using System.ComponentModel.DataAnnotations;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;


public class MedicalReviewDto
{
    [Required(ErrorMessage = "Medical Review Assignment Id is required")]
    public Guid MedicalReviewAssignmentId { get; set; }

    [Required(ErrorMessage = "Clinical Significance is required")]
    public ClinicalSignificance? ClinicalSignificance { get; set; }

    [Required(ErrorMessage = "Causality is required")]
    public CausalityLevel? Causality { get; set; }

    [Required(ErrorMessage = "Reviewed At is required.")]
    public DateTime? ReviewedAt { get; set; }

    [Required(ErrorMessage = "At least one Clinical Medical Review is required.")]
    [MinLength(1, ErrorMessage = "At least one Clinical Medical Review must be provided.")]
    public List<ClinicalMedicalReviewDto> ClinicalMedicalReviews { get; set; } = new();
}

public class ClinicalMedicalReviewDto
{
    [Required(ErrorMessage = "Adverse Event Id is required")]
    public Guid AdverseEventId { get; set; }

    [StringLength(300, ErrorMessage = "Description cannot exceed 300 characters.")]
    public string? LaboratoryResults { get; set; }

    [StringLength(200, ErrorMessage = "MedDRACode cannot exceed 200 characters.")]
    public string? MedDRACode { get; set; }

    [StringLength(200, ErrorMessage = "RetClassification cannot exceed 200 characters.")]
    public string? RetClassification { get; set; }
}