namespace Finlay.PharmaVigilance.Domain.Entities;

public class Alert : GuidEntity
{
    public string Description { get; set; } = null!;
    public DateTime? ReadAt { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsRead { get; set; } = false;


    //FK
    public Guid SectionResponsibleId { get; set; }
    public SectionResponsible SectionResponsible { get; set; } = null!;
    public Guid AefiReportId { get; set; }
    public AefiReport AefiReport { get; set; } = null!;
}