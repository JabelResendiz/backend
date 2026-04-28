

using System.ComponentModel.DataAnnotations;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;


public class AdverseEventDto
{

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime? StartDate { get; set; }

    [Required(ErrorMessage = "Adverse Event description is required.")]
    [StringLength(500, MinimumLength = 1)]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "VisitedDoctor is required.")]
    public bool VisitedDoctor { get; set; }

    [Required(ErrorMessage = "WentToEmergency is required.")]
    public bool WentToEmergencyRoom { get; set; }

    [Required(ErrorMessage = "Permanent Disability is required.")]
    public bool PermanentDisability { get; set; }

    [Required(ErrorMessage = "Is Life Threatening is required.")]
    public bool IsLifeThreatening { get; set; }

    [Required(ErrorMessage = "Resulted In Death is required.")]
    public bool ResultedInDeath { get; set; }
    public DateTime? DeathDate { get; set; }

    [Required(ErrorMessage = "Current Status is required.")]
    public PatientStatus? CurrentStatus { get; set; }

    [StringLength(300, MinimumLength = 1, ErrorMessage = "Laboratory results must be between 1 and 300 characters.")]
    public string? LaboratoryResults { get; set; }

    [StringLength(200, MinimumLength = 1, ErrorMessage = "MedDRACode must be between 1 and 200 characters.")]
    public string? MedDRACode { get; set; }

    [StringLength(200, MinimumLength = 1, ErrorMessage = "Ret Classification must be between 1 and 200 characters.")]
    public string? RetClassification { get; set; }


    [Required(ErrorMessage = "At least one symptom is required.")]
    [MinLength(1, ErrorMessage = "At least one symptom must be provided.")]
    public List<Guid> Symptoms { get; set; } = new();

}