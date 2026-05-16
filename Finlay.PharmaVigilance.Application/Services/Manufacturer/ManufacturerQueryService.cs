using AutoMapper;
using AutoMapper.QueryableExtensions;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Finlay.PharmaVigilance.Application.Services;

public class ManufacturerQueryService : IManufacturerQueryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ManufacturerQueryService> _logger;

    public ManufacturerQueryService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<ManufacturerQueryService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<ManufacturerResponseDto>> GetManufacturers()
    {
        var manufacturers = await _unitOfWork.GetRepository<Manufacturer>()
                                .GetAll()
                                .ProjectTo<ManufacturerResponseDto>(_mapper.ConfigurationProvider)
                                .ToListAsync();

        return manufacturers;
    }

}