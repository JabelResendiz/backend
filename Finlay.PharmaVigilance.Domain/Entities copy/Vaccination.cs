using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Domain.Entities;


public class Vaccination : GuidEntity
{
    public string BatchNumber { get; set; } = null!;
    public AdministrationSite Site { get; set; }
    public int DoseNumber { get; set; }
    public DateTime AdministrationDate { get; set; }
    public string? VaccinationCenter { get; set; }

    public Guid VaccineId { get; set; }
    public Vaccine Vaccine { get; set; } = null!;
    public Guid AefiReportId { get; set; }
    public AefiReport AefiReport { get; set; } = null!;

}