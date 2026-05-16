using System.Data.Common;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Application.Services;


public class VaccineQueryService : GenericQueryService<Vaccine, GetVaccineDto>,
                                         IVaccineQueryService
{
    private readonly IVaccineRepository _vaccineRepository;

    public VaccineQueryService(IUnitOfWork unitOfWork, IMapper mapper, IVaccineRepository vaccineRepository)
        : base(unitOfWork, mapper)
    {
        _vaccineRepository = vaccineRepository;
    }

    public async Task<PagedResultDto<GetVaccineDto>> GetActivesVaccine(PagedRequestDto paged)
    {
        var query = _unitOfWork.GetRepository<Vaccine>()
                    .GetAllByItems(v => v.IsActive);

        var totalCount = await query.CountAsync();

        var items = await _unitOfWork.GetRepository<Vaccine>()
                        .GetAllPaged((paged.PageNumber - 1) * paged.PageSize, paged.PageSize)
                        .ToListAsync();

        return new PagedResultDto<GetVaccineDto>
        {
            Items = items?.Select(_mapper.Map<GetVaccineDto>) ?? Enumerable.Empty<GetVaccineDto>(),
            TotalCount = totalCount,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize,
            NextPageUrl = paged.PageNumber * paged.PageSize < totalCount
                        ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber + 1}&pageSize={paged.PageSize}"
                        : null,
            PreviousPageUrl = paged.PageNumber > 1
                        ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber - 1}&pageSize={paged.PageSize}"
                        : null

        };
    }


    public async Task<IEnumerable<GetVaccineDto>> GetActiveVaccinesLookup()
    {
        return await _vaccineRepository
                    .GetAllByItems(v => v.IsActive)
                    .ProjectTo<GetVaccineDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
    }


    public async Task<PagedResultDto<GetPrivateVaccineDto>> GetByFilters(PagedRequestDto paged, string? search, bool? status)
    {

        var query = _vaccineRepository.GetByFilter(search, status);

        var totalCount = await query.CountAsync();


        var items = await _vaccineRepository.GetPaged(query, (paged.PageNumber - 1) * paged.PageSize, paged.PageSize)
                            .ToListAsync();

        return new PagedResultDto<GetPrivateVaccineDto>
        {
            Items = items?.Select(_mapper.Map<GetPrivateVaccineDto>) ?? Enumerable.Empty<GetPrivateVaccineDto>(),
            TotalCount = totalCount,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize,
            NextPageUrl = paged.PageNumber * paged.PageSize < totalCount
                       ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber + 1}&pageSize={paged.PageSize}"
                       : null,
            PreviousPageUrl = paged.PageNumber > 1
                       ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber - 1}&pageSize={paged.PageSize}"
                       : null

        };

    }


    public async Task<ICollection<VaccineDashboardDto>> GetVaccinesDashboard()
    {

        var query = await _unitOfWork.GetRepository<Vaccine>()
                            .GetAll()
                            .ProjectTo<VaccineDashboardDto>(_mapper.ConfigurationProvider)
                            .ToListAsync();


        for (int i = 0; i < query.Count(); i++)
        {
            query[i].TotalReport = await _unitOfWork.GetRepository<Vaccination>()
                                    .GetAllByItems(vac => vac.Lot.Vaccine.Name == query[i].Name)
                                    .CountAsync();
        }



        return query;
    }


    public async Task<IEnumerable<GetVaccineDto>> GetSelfVaccines()
    {
        var vaccines = await _unitOfWork.GetRepository<Vaccine>()
                        .GetAllByItems(v => v.Manufacturer.Name == "IFV")
                        .ProjectTo<GetVaccineDto>(_mapper.ConfigurationProvider)
                        .ToListAsync();

        return vaccines;
    }
}