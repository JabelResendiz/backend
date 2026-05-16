using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IRepository;

public interface IReportRepository : IGenericRepository<AefiReport>
{
    IQueryable<AefiReport> GetByFilter(
        string? vaccineName,
        string? provinceName,
        string? severity,
        string? reportStatus);


    IQueryable<AefiReport> GetSectionResponsibleByFilter(
    IQueryable<AefiReport> query,
ReportSectionResponsibleFilter filter);

}