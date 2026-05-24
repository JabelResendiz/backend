

using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class VaccinationRepository : GenericRepository<Vaccination>, IVaccinationRepository
{
    public VaccinationRepository(FinlayDbContext context) : base(context) { }


    public async Task<IEnumerable<VaccineStatsDto>> GetVaccineByFilter(int municipalityId)
    {
        var result = await _entity
            .Include(vac => vac.AefiReport)
            .Include(vac => vac.Lot)
                .ThenInclude(l => l.Vaccine)
            .Where(x => x.AefiReport.VaccinatedSubject.MunicipalityId == municipalityId)
            .GroupBy(vac => vac.Lot.Vaccine.Name)
            .Select(g => new VaccineStatsDto
            {
                VaccineName = g.Key,
                TotalReports = g.Select(vac => vac.AefiReport.Id).Distinct().Count()
            })
            .ToListAsync();

        foreach (var i in result)
        {
            Console.WriteLine($"{i.VaccineName} ---> {i.TotalReports}");
        }


        return result;

    }
}