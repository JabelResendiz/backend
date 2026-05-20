
namespace Finlay.PharmaVigilance.Domain.Entities;


public class MedicalReviewer : GuidEntity
{
   public int ProvinceId { get; set; }
   public Province Province { get; set; } = null!;
   public int MunicipalityId { get; set; }
   public Municipality Municipality { get; set; } = null!;
   public string Institution { get; set; } = null!;

   public string IdentityNumber { get; set; } = null!;
   public DateTime DateOfBirth => ExtractDateOfBirth(IdentityNumber);
   // FK
   public Guid UserId { get; set; }
   public User User { get; set; } = null!;
   public string ProfessionalLicense { get; set; } = null!;
   public string? Specialty { get; set; }

   public Guid SectionResponsibleId { get; set; }
   public SectionResponsible SectionResponsible { get; set; } = null!;

   public ICollection<MedicalReviewAssignment> MedicalReviews { get; set; } = new List<MedicalReviewAssignment>();




   private static DateTime ExtractDateOfBirth(string identityNumber)
   {
      string yy = identityNumber.Substring(0, 2);
      string mm = identityNumber.Substring(2, 2);
      string dd = identityNumber.Substring(4, 2);

      int year = int.Parse(yy);
      int month = int.Parse(mm);
      int day = int.Parse(dd);

      int currentYearTwoDigits = DateTime.Now.Year % 100;
      int fullYear = (year > currentYearTwoDigits) ? 1900 + year : 2000 + year;

      DateTime extractedDate;

      try
      {
         extractedDate = new DateTime(fullYear, month, day);
      }
      catch
      {
         throw new ArgumentException("Invalid date encoded in identity number.");
      }

      return extractedDate;
   }

}