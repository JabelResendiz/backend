using System.Linq.Expressions;
using AutoMapper;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IServices.Common;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Application.Validators;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;
using Finlay.PharmaVigilance.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Finlay.PharmaVigilance.Application.Services;

public class VaccinationCenterCommandService : IVaccinationCenterCommandService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<VaccinationCenterCommandService> _logger;


    public VaccinationCenterCommandService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<VaccinationCenterCommandService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task CreateVaccinationCenter(VaccinationCenterDto vaccinationCenterDto)
    {
        try
        {
            var municipality = await _unitOfWork.GetRepository<Municipality>()
                            .GetByIdAsync(vaccinationCenterDto.MunicipalityId);

            if (municipality.ProvinceId != vaccinationCenterDto.ProvinceId)
            {
                _logger.LogError($"Municipality {municipality.Id} does not belong to province {vaccinationCenterDto.ProvinceId}.");

                throw new ArgumentException(
                    $"Municipality {municipality.Id} does not belong to province {vaccinationCenterDto.ProvinceId}.",
                    nameof(vaccinationCenterDto.MunicipalityId));
            }

            var vaccinationCenter = _mapper.Map<VaccinationCenter>(vaccinationCenterDto);


            await _unitOfWork.GetRepository<VaccinationCenter>().CreateAsync(vaccinationCenter);
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating vaccination center");


            throw new InvalidOperationException(
                $"Error to create vaccination center: {ex.Message}",
                ex);
        }



    }

}