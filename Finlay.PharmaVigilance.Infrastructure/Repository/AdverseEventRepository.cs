using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class AdverseEventRepository : GenericRepository<AdverseEvent>, IAdverseEventRepository
{
    public AdverseEventRepository(FinlayDbContext context) : base(context) { }



    public async Task<IEnumerable<SymptomStatsDto>> GetSymptomFilter(int municipalityId)
    {
        var result = await _entity
            .Include(ad => ad.AefiReport)
            .Include(ad => ad.Symptom)
            .Where(ad => ad.AefiReport.VaccinatedSubject.MunicipalityId == municipalityId)
            .GroupBy(ad => ad.Symptom.Name)
            .Select(x => new SymptomStatsDto
            {
                SymptomName = x.Key,
                TotalReports = x.Select(vac => vac.AefiReport.Id).Distinct().Count()
            })
            .ToListAsync();

        foreach (var i in result)
        {
            Console.WriteLine($"{i.SymptomName} ---> {i.TotalReports}");
        }

        return result;
    }

    public async Task<IEnumerable<SeverityDistributionDto>> GetSeverityDistribution(int municipalityId)
    {
        var result = await _entity
            .Include(ad => ad.AefiReport)
            .Where(ad => ad.AefiReport.VaccinatedSubject.MunicipalityId == municipalityId)
            .GroupBy(ad => ad.AefiReport.Id)
            .Select(g => new
            {
                MaxSeverity = g.Max(ad => ad.SeverityLevel)
            })
            .GroupBy(x => x.MaxSeverity)
            .Select(x => new SeverityDistributionDto
            {
                Severity = x.Key.ToString(),
                TotalReports = x.Count()
            })
            .ToListAsync();


        foreach (var i in result)
        {
            Console.WriteLine($"{i.Severity}----> {i.TotalReports}");
        }

        return result;
    }

    public async Task<SeriousDataDto> GetSeriousDataAsync(int municipalityId)
    {
        var result = await _entity
            .Where(ad => ad.AefiReport.VaccinatedSubject.MunicipalityId == municipalityId)
            .GroupBy(ad => 1)
            .Select(g => new SeriousDataDto
            {
                VisitedDoctor = g.Count(x => x.VisitedDoctor),
                WentToEmergencyRoom = g.Count(x => x.WentToEmergencyRoom),
                PermanentDisability = g.Count(x => x.PermanentDisability),
                Anomaly = g.Count(x => x.Anomaly),
                WasHospitalized = g.Count(x => x.WasHospitalized),
                ResultedInDeath = g.Count(x => x.ResultedInDeath),
                NoComplications = g.Count(x => x.NoComplications)
            })
            .FirstOrDefaultAsync() ?? new SeriousDataDto();

        return result;
    }






    public async Task<IEnumerable<SymptomDistributionDto>> GetSymptomDistributionAsync()
    {
        var data = await _entity
            .GroupBy(e => e.Symptom.Name)
            .Select(g => new
            {
                SymptomName = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

        var total = data.Sum(x => x.Count);

        var result = data
            .Select(x => new SymptomDistributionDto
            {
                SymptomName = x.SymptomName,
                Count = x.Count,
                Percentage = total > 0
                    ? Math.Round((double)x.Count * 100 / total, 2)
                    : 0
            })
            .ToList();

        return result;
    }
}

