using AutoMapper;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Finlay.PharmaVigilance.Application.Services;

public class CatalogCommandService : ICatalogCommandService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CatalogCommandService> _logger;


    public CatalogCommandService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CatalogCommandService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> CreateVaccineAsync(VaccineDto vaccineDto)
    {
        if (vaccineDto == null)
            throw new ArgumentNullException(nameof(vaccineDto));

        // Validate approval date is in the past (UTC-5 Cuba timezone)
        if (vaccineDto.ApprovalDate.HasValue)
        {
            TimeZoneInfo cubaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime cubaTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, cubaTimeZone);

            if (vaccineDto.ApprovalDate >= cubaTime)
                throw new InvalidOperationException("Approval date must be in the past (Cuba timezone UTC-5).");
        }

        try
        {
            var vaccine = _mapper.Map<Vaccine>(vaccineDto);

            await _unitOfWork.GetRepository<Vaccine>().CreateAsync(vaccine);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Vaccine successfully created");

            return "Vaccine successfully created";
        }
        catch (DbUpdateException ex)
        {
            // Check if it's a unique constraint violation on NormalizedName
            if (ex.InnerException?.Message.Contains("Duplicate entry") == true &&
                ex.InnerException?.Message.Contains("IX_Vaccines_NormalizedName") == true)
            {
                throw new InvalidOperationException(
                    $"A vaccine with the name '{vaccineDto.Name}' already exists. " +
                    $"Vaccine names are case-insensitive and ignore accents. " +
                    $"Please use a different name.",
                    ex);
            }

            throw;
        }
    }

    public async Task<string> CreateSymptomAsync(SymptomDto symptomDto)
    {
        if (symptomDto == null)
            throw new ArgumentNullException(nameof(symptomDto));

        try
        {
            var symptom = _mapper.Map<Symptom>(symptomDto);

            await _unitOfWork.GetRepository<Symptom>().CreateAsync(symptom);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation(
                "Symptom created: {Name}, Category: {Category}, Code: {Code}",
                symptomDto.Name,
                symptomDto.Category,
                symptomDto.StandardCode
            );

            return "Symptom successfully created";
        }
        catch (DbUpdateException ex)
        {
            // Check if it's a unique constraint violation on NormalizedName
            if (ex.InnerException?.Message.Contains("Duplicate entry") == true &&
                ex.InnerException?.Message.Contains("IX_Symptoms_NormalizedName") == true)
            {
                throw new InvalidOperationException(
                    $"A symptom with the name '{symptomDto.Name}' already exists. " +
                    $"Symptom names are case-insensitive and ignore accents. " +
                    $"Please use a different name.",
                    ex);
            }

            throw;
        }
    }

    public async Task<string> DeactivateVaccine(int vaccineId)
    {
        var vaccine = await _unitOfWork.GetRepository<Vaccine>()
                        .GetByIdAsync(vaccineId);

        if (vaccine == null)
            throw new InvalidOperationException("This vaccine not exists");

        vaccine.IsActive = false;

        await _unitOfWork.CompleteAsync();

        return "Vaccine is now inactive";
    }
    public async Task<string> DeactivateSymptom(int symptomId)
    {
        var symptom = await _unitOfWork.GetRepository<Symptom>()
                        .GetByIdAsync(symptomId);

        if (symptom == null)
            throw new InvalidOperationException("This symptom not exists");

        symptom.IsActive = false;

        await _unitOfWork.CompleteAsync();

        return "Symptom is now inactive";
    }

    public async Task<string> ActivateVaccine(int vaccineId)
    {
        var vaccine = await _unitOfWork.GetRepository<Vaccine>()
                                .GetByIdAsync(vaccineId);

        if (vaccine == null)
            throw new InvalidOperationException("This vaccine not exists");

        vaccine.IsActive = true;

        await _unitOfWork.CompleteAsync();

        return "Vaccine is now active";
    }
    public async Task<string> ActivateSymptom(int symptomId)
    {
        var symptom = await _unitOfWork.GetRepository<Symptom>()
                                .GetByIdAsync(symptomId);

        if (symptom == null)
            throw new InvalidOperationException("This symptom not exists");

        symptom.IsActive = true;

        await _unitOfWork.CompleteAsync();

        return "Symptom is now active";
    }
}