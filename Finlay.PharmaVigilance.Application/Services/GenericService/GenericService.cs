using System.Linq.Expressions;
using AutoMapper;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Application.Services;


public class GenericQueryService<TEntity, TDto> : IGenericQueryService<TEntity, TDto> where TEntity : BasicEntity
{
    protected readonly IMapper _mapper;
    protected readonly IUnitOfWork _unitOfWork;
    public GenericQueryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public virtual Expression<Func<TEntity, object>>[] GetIncludes()
    {
        return Array.Empty<Expression<Func<TEntity, object>>>();
    }

    public async Task<IEnumerable<TDto>> ListAsync()
    {
        var includes = GetIncludes();
        var dtoQuery = _unitOfWork.GetRepository<TEntity>().GetAll();

        if (includes != null)
        {
            foreach (var exp in includes) // Loop through each filter expression.
            {
                dtoQuery = dtoQuery.Include(exp); // Apply the filter expression to the query.
            }
        }

        var dtoList = await dtoQuery.ToListAsync();

        return dtoList.Select(_mapper.Map<TDto>);
    }


    public async Task<TDto> GetByIdAsync<TId>(TId dto)
    {
        var includes = GetIncludes();

        var result = await _unitOfWork.GetRepository<TEntity>()
                                      .GetByIdAsync(dto, default, includes);

        return _mapper.Map<TDto>(result);

    }


    public async Task<PagedResultDto<TDto>> GetAllPagedResultAsync(PagedRequestDto paged)
    {
        var query = _unitOfWork.GetRepository<TEntity>()
                        .GetAll();

        var includes = GetIncludes();

        if (includes != null)
        {
            foreach (var exp in includes) // Loop through each filter expression.
            {
                query = query.Include(exp); // Apply the filter expression to the query.
            }
        }

        var totalCount = await query.CountAsync();


        var items = await _unitOfWork.GetRepository<TEntity>()
                        .GetAllPaged((paged.PageNumber - 1) * paged.PageSize, paged.PageSize)
                        .ToListAsync();


        return new PagedResultDto<TDto>
        {
            Items = items?.Select(_mapper.Map<TDto>) ?? Enumerable.Empty<TDto>(),
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