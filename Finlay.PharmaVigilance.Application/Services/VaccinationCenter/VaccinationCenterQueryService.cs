using AutoMapper;
using AutoMapper.QueryableExtensions;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IServices.Common;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Finlay.PharmaVigilance.Application.Services;

public class VaccinationCenterQueryService : IVaccinationCenterQueryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<VaccinationCenterQueryService> _logger;
    private readonly IUserContextService _userContextService;


    public VaccinationCenterQueryService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<VaccinationCenterQueryService> logger,
        IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userContextService = userContextService ?? throw new ArgumentNullException(nameof(userContextService));
    }

    public async Task<IEnumerable<VaccinationCenterResponseDto>> GetByMunicipality(int municipalityId, int provinceId)
    {
        try
        {
            var vaccinationCenters = await _unitOfWork.GetRepository<VaccinationCenter>()
                                .GetAllByItems(vc => vc.MunicipalityId == municipalityId && vc.ProvinceId == provinceId)
                                .ProjectTo<VaccinationCenterResponseDto>(_mapper.ConfigurationProvider)
                                .ToListAsync();

            return vaccinationCenters;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving vaccination centers by municipality");

            throw new InvalidOperationException(
                $"Error retrieving vaccination centers by municipality: {ex.Message}",
                ex);
        }
    }


    public async Task<IEnumerable<VaccinationCenterResponseDto>> GetBySectionResponsible()
    {
        try
        {

            var userContext = _userContextService.GetUserId();

            var sectionResponsible = await _unitOfWork.GetRepository<SectionResponsible>()
                                .FirstOrDefaultAsync(sr => sr.UserId == userContext)
                                ?? throw new InvalidOperationException($"Section responsible not found for this user ID");

            var vaccinationCenters = await _unitOfWork.GetRepository<VaccinationCenter>()
                                .GetAllByItems(vc => vc.MunicipalityId == sectionResponsible.MunicipalityId &&
                                                     vc.ProvinceId == sectionResponsible.ProvinceId)
                                .ProjectTo<VaccinationCenterResponseDto>(_mapper.ConfigurationProvider)
                                .ToListAsync();

            return vaccinationCenters;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving vaccination centers by section responsible");

            throw new InvalidOperationException(
                $"Error retrieving vaccination centers by section responsible: {ex.Message}",
                ex);
        }
    }
}
