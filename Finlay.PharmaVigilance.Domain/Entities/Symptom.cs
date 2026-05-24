using Finlay.PharmaVigilance.Domain.ValueObjects;

namespace Finlay.PharmaVigilance.Domain.Entities;

public class Symptom : GuidEntity
{
    private string _name = null!;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            NormalizedName = NameNormalizer.Normalize(value);
        }
    }

    public string? NormalizedName { get; private set; }

    public string? Description { get; set; }
    public string Category { get; set; } = null!;
    public bool IsActive { get; set; }

    public ICollection<AdverseEvent> AdverseEvents { get; set; } = new List<AdverseEvent>();


}