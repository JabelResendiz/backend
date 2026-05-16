using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Domain.Entities;


public class Vaccination : GuidEntity
{
    public AdministrationSite Site { get; set; }
    public int DoseNumber { get; set; }
    public DateTime AdministrationDate { get; set; }
    public Guid VaccinationCenterId { get; set; }
    public VaccinationCenter VaccinationCenter { get; set; } = null!;
    public Guid LotId { get; set; }
    public Lot Lot { get; set; } = null!;

    public Guid AefiReportId { get; set; }
    public AefiReport AefiReport { get; set; } = null!;

}