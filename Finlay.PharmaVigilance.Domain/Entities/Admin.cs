namespace Finlay.PharmaVigilance.Domain.Entities;

public class Admin : GuidEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public ICollection<SectionResponsible> RegisteredResponsibles { get; set; } = new List<SectionResponsible>();

}