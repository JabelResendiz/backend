
namespace Finlay.PharmaVigilance.Domain.Entities;

public class AdverseEventSymptom : GuidEntity
{
    public Guid AdverseEventId { get; set; }
    public AdverseEvent AdverseEvent { get; set; } = null!;
    public Guid SymptomId { get; set; }
    public Symptom Symptom { get; set; } = null!;

}