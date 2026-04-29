namespace Finlay.PharmaVigilance.Domain.Entities;

public class SectionResponsible : GuidEntity
{
    public int ProvinceId { get; set; }
    public Province Province { get; set; } = null!;
    public int MunicipalityId { get; set; }
    public Municipality Municipality { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public ICollection<Alert> ReceivedAlerts { get; set; } = new List<Alert>();
    public ICollection<MedicalReviewAssignment> ManagedReviews { get; set; } = new List<MedicalReviewAssignment>();
    public ICollection<MedicalReviewer> MedicalReviewers { get; set; } = new List<MedicalReviewer>();
    public Guid AdminId { get; set; }
    public Admin Admin { get; set; } = null!;

}