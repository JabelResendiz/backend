
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class VaccinatedSubjectDetailsDto
{
    public required string FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }

}

public class VaccinatedSubjectAdminDto
{
    public required int Age { get; set; }
    public required Gender Gender { get; set; }
    public required bool? IsPregnant { get; set; }
    public required string ProvinceName { get; set; }
    public required string MunicipalityName { get; set; }
    public required string? CurrentMedications { get; set; }
    public required string? Allergies { get; set; }
    public required string? MedicalHistory { get; set; }

}