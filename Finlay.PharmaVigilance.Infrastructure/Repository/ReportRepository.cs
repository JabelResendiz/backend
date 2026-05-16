

using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class ReportRepository : GenericRepository<AefiReport>, IReportRepository
{
    public ReportRepository(FinlayDbContext context) : base(context) { }

    public IQueryable<AefiReport> GetByFilter(
        string? vaccineName,
        string? provinceName,
        string? severity,
        string? reportStatus)
    {
        var query = _entity.AsQueryable();

        if (!string.IsNullOrWhiteSpace(vaccineName))
        {
            query = query.Where(ar => ar.Vaccinations.Any(v => v.Lot.Vaccine.Name == vaccineName));
        }

        if (!string.IsNullOrWhiteSpace(provinceName))
        {
            query = query.Where(ar => ar.VaccinatedSubject.Province.Name == provinceName);
        }

        if (!string.IsNullOrWhiteSpace(severity))
        {
            if (Enum.TryParse<SeverityLevel>(severity, true, out var severityEnum))
            {
                query = query.Where(r =>
                    r.AdverseEvents.Any(a =>
                        a.SeverityLevel == severityEnum));
            }

        }

        if (!string.IsNullOrWhiteSpace(reportStatus))
        {

            if (Enum.TryParse<ReportStatus>(reportStatus, true, out var statusEnum))
            {
                query = query.Where(r => r.Status == statusEnum);
            }
        }

        return query;

    }


    public IQueryable<AefiReport> GetSectionResponsibleByFilter(
        IQueryable<AefiReport> query,
        ReportSectionResponsibleFilter filter)
    {

        if (!string.IsNullOrWhiteSpace(filter.VaccineName))
        {
            query = query.Where(ar =>
            ar.Vaccinations.Any(v => v.Lot.Vaccine.Name == filter.VaccineName));
        }

        if (filter.VaccinationCenterId != null)
        {
            query = query.Where(ar => ar.Vaccinations.Any(v => v.VaccinationCenterId == filter.VaccinationCenterId));
        }

        if (!string.IsNullOrWhiteSpace(filter.Severity))
        {
            if (Enum.TryParse<SeverityLevel>(filter.Severity, true, out var severityEnum))
            {
                query = query.Where(r =>
                    r.AdverseEvents.Any(a =>
                        a.SeverityLevel == severityEnum));
            }

        }

        if (filter.From.HasValue)
        {
            query = query.Where(r => r.ReportDate >= filter.From.Value);
        }

        if (filter.To.HasValue)
        {
            query = query.Where(r => r.ReportDate <= filter.To.Value);
        }

        bool asc = filter.Order?.ToLower() == "asc";

        Console.WriteLine($"=================================={filter.SortBy?.ToLower()}===============================");

        query = filter.SortBy?.ToLower() switch
        {
            "reportdate" => asc
                ? query.OrderBy(r => r.ReportDate)
                : query.OrderByDescending(r => r.ReportDate),

            "vaccinatedsubject.fullname" => asc
                ? query.OrderBy(r => r.VaccinatedSubject.FullName)
                : query.OrderByDescending(r => r.VaccinatedSubject.FullName),

            _ => query.OrderByDescending(r => r.ReportDate) // default
        };

        return query;

    }
}