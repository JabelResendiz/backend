

using System.ComponentModel.DataAnnotations;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;


public class AdverseEventDto
{

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime? StartDate { get; set; }

    [Required(ErrorMessage = "Finish date is required.")]
    public DateTime? FinishDate { get; set; }

    [StringLength(500, MinimumLength = 0)]
    public string? Description { get; set; }

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

    [Required(ErrorMessage = "Intensity is required.")]
    public Intensity? Intensity { get; set; }

    [Required(ErrorMessage = "Severity Level is required.")]
    public SeverityLevel? SeverityLevel { get; set; }

    [StringLength(300, MinimumLength = 1, ErrorMessage = "Laboratory results must be between 1 and 300 characters.")]
    public string? LaboratoryResults { get; set; }

    [StringLength(200, MinimumLength = 1, ErrorMessage = "MedDRACode must be between 1 and 200 characters.")]
    public string? MedDRACode { get; set; }

    [StringLength(200, MinimumLength = 1, ErrorMessage = "Ret Classification must be between 1 and 200 characters.")]
    public string? RetClassification { get; set; }


    [Required(ErrorMessage = "Symptom is required")]
    public Guid SymptomId { get; set; }

}