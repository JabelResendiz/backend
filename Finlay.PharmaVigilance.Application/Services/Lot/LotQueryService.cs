using AutoMapper;
using AutoMapper.QueryableExtensions;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Finlay.PharmaVigilance.Application.Services;

public class LotQueryService : ILotQueryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<LotQueryService> _logger;

    public LotQueryService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<LotQueryService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<LotResponseDto>> GetByVaccine(Guid vaccineId)
    {
        try
        {
            var lots = await _unitOfWork.GetRepository<Lot>()
                            .GetAllByItems(l => l.VaccineId == vaccineId)
                            .ProjectTo<LotResponseDto>(_mapper.ConfigurationProvider)
                            .ToListAsync();

            return lots;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating lot");


            throw new InvalidOperationException(
                $"Error to create vaccination center: {ex.Message}",
                ex);
        }

    }

}