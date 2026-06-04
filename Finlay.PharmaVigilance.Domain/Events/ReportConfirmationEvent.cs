namespace Finlay.PharmaVigilance.Domain.Enum;

public class ReportConfirmationEvent
{
    public string Email { get; set; } = default!;
    public string ReportNumber { get; set; } = null!;
    public ICollection<Guid> SymptomIds { get; set; }
        = new List<Guid>();
    public ICollection<Guid> VaccineIds { get; set; }
        = new List<Guid>();
    public DateTime ReportDate { get; set; }
}