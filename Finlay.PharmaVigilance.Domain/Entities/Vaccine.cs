using Finlay.PharmaVigilance.Domain.Enum;
using Finlay.PharmaVigilance.Domain.ValueObjects;

namespace Finlay.PharmaVigilance.Domain.Entities;

public class Vaccine : GuidEntity
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

    public VaccineType Type { get; set; }
    public string? Description { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid ManufacturerId { get; set; }
    public Manufacturer Manufacturer { get; set; } = null!;
    public string TargetPathology { get; set; } = null!;
    public ICollection<Vaccination> Vaccinations { get; set; } = new List<Vaccination>();
    public ICollection<Lot> Lots { get; set; } = new List<Lot>();

}