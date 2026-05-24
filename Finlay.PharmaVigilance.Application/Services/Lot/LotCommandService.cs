
using System.Data;
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

public class LotCommandService : ILotCommandService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<LotCommandService> _logger;


    public LotCommandService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<LotCommandService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Create(LotDto lotDto)
    {
        try
        {
            var vaccineId = await _unitOfWork.GetRepository<Vaccine>()
                            .GetByIdAsync(lotDto.VaccineId)
                            ?? throw new KeyNotFoundException(
                                        $"Vaccine with ID {lotDto.VaccineId} not found in the database.");

            var lotName = await _unitOfWork.GetRepository<Lot>()
                                .FirstOrDefaultAsync(l => l.LotNumber == lotDto.LotNumber);

            if (lotName != null)
                throw new DuplicateNameException("Lot NUmber is duplicated");

            var lot = _mapper.Map<Lot>(lotDto);


            await _unitOfWork.GetRepository<Lot>().CreateAsync(lot);
            await _unitOfWork.CompleteAsync();
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