
namespace Finlay.PharmaVigilance.Application.DTO;

public class GetMedicalReviewerDto
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int ProvinceId { get; set; }
    public int MunicipalityId { get; set; }
    public string Institution { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;

}