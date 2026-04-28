
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Domain.Entities;

public class Reporter : GuidEntity
{
    public string FullName { get; set; } = null!;
    public ReporterRelationship ReporterRelationship { get; set; }
    public string IdentityNumber { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }

    public int ProvinceId { get; set; }
    public Province Province { get; set; } = null!;
    public int MunicipalityId { get; set; }
    public Municipality Municipality { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string? ProfessionalLicense { get; set; }
    public string? Institution { get; set; }

    public int? UserId { get; set; }
    public User? User { get; set; } = null!;

    public ICollection<AefiReport> AefiReports { get; set; } = new List<AefiReport>();
}