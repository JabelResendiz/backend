
using Finlay.PharmaVigilance.Domain.ValueObjects;

namespace Finlay.PharmaVigilance.Domain.Entities;


public class MedicalReviewer : GuidEntity
{
   public int ProvinceId { get; set; }
   public Province Province { get; set; } = null!;
   public int MunicipalityId { get; set; }
   public Municipality Municipality { get; set; } = null!;
   public string Institution { get; set; } = null!;

   public IdentityNumber IdentityNumber { get; set; } = null!;

   // FK
   public Guid UserId { get; set; }
   public User User { get; set; } = null!;
   public string ProfessionalLicense { get; set; } = null!;
   public string? Specialty { get; set; }

   public Guid SectionResponsibleId { get; set; }
   public SectionResponsible SectionResponsible { get; set; } = null!;

   public ICollection<MedicalReviewAssignment> MedicalReviews { get; set; } = new List<MedicalReviewAssignment>();

}