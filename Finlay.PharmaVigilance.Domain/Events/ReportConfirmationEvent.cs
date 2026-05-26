namespace Finlay.PharmaVigilance.Domain.Enum;

public class ReportConfirmationEvent
{
    public string Email { get; set; } = default!;
    public string ReportNumber { get; set; } = null!;
    public ICollection<string> SymptomsName { get; set; }
        = new List<string>();
    public ICollection<string> VaccinesName { get; set; }
        = new List<string>();
    public DateTime ReportDate { get; set; }
}