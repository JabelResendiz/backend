using AutoMapper;
using AutoMapper.QueryableExtensions;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Application.Services;


public class SymptomQueryService : GenericQueryService<Symptom, GetSymptomDto>,
                                         ISymptomQueryService
{

    private readonly ISymptomRepository _symptomRepository;

    public SymptomQueryService(IUnitOfWork unitOfWork, IMapper mapper, ISymptomRepository symptomRepository)
        : base(unitOfWork, mapper)
    {
        _symptomRepository = symptomRepository;
    }

    public async Task<PagedResultDto<GetSymptomDto>> GetActivesSymptoms(PagedRequestDto paged)
    {
        var query = _unitOfWork.GetRepository<Symptom>()
                    .GetAllByItems(v => v.IsActive);

        var totalCount = await query.CountAsync();

        var items = await _unitOfWork.GetRepository<Symptom>()
                        .GetAllPaged((paged.PageNumber - 1) * paged.PageSize, paged.PageSize)
                        .ToListAsync();

        return new PagedResultDto<GetSymptomDto>
        {
            Items = items?.Select(_mapper.Map<GetSymptomDto>) ?? Enumerable.Empty<GetSymptomDto>(),
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


    public async Task<IEnumerable<GetSymptomDto>> GetActiveSymptomsLookup()
    {
        return await _symptomRepository
                    .GetAllByItems(v => v.IsActive)
                    .ProjectTo<GetSymptomDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
    }

    public async Task<PagedResultDto<GetSymptomDto>> GetByFilters(PagedRequestDto paged, string? search, bool? status)
    {

        var query = _symptomRepository.GetByFilter(search, status);

        var totalCount = await query.CountAsync();


        var items = await _symptomRepository.GetPaged(query, (paged.PageNumber - 1) * paged.PageSize, paged.PageSize)
                            .ToListAsync();

        return new PagedResultDto<GetSymptomDto>
        {
            Items = items?.Select(_mapper.Map<GetSymptomDto>) ?? Enumerable.Empty<GetSymptomDto>(),
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

}