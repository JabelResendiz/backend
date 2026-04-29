using System.Linq.Expressions;
using AutoMapper;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IServices.Common;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Application.Helpers;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Application.Services;

public class MedicalReviewCommandService : IMedicalReviewCommandService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public MedicalReviewCommandService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<MedicalReviewDto> CreateAsync(MedicalReviewDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        if (!EnumHelper<CausalityLevel>.IsValid(dto.Causality.ToString()!))
        {
            throw new ArgumentException(
                                          "Causality Level must be valid",
                                          nameof(dto.Causality)
                                      );
        }

        if (!EnumHelper<ClinicalSignificance>.IsValid(dto.ClinicalSignificance.ToString()!))
        {
            throw new ArgumentException(
                                          "Clinical Significance must be valid",
                                          nameof(dto.ClinicalSignificance)
                                      );
        }

        var userId = _userContextService.GetUserId();

        var medicalReviewer = await _unitOfWork.GetRepository<MedicalReviewer>()
                                        .FirstOrDefaultAsync(sr => sr.UserId == userId);

        if (medicalReviewer == null)
            throw new UnauthorizedAccessException("User is not a medical reviewer.");

        var medicalAssignment = await _unitOfWork.GetRepository<MedicalReviewAssignment>()
                                .GetByIdAsync(dto.MedicalReviewAssignmentId);

        if (medicalAssignment == null)
            throw new KeyNotFoundException("Medical Review Assignment not found.");

        if (medicalAssignment.MedicalReviewerId != medicalReviewer.Id)
            throw new UnauthorizedAccessException("This assignment does not belong to the current medical reviewer.");

        var easternNow = TimeZoneHelper.GetEasternNow();

        Console.WriteLine($"============================0Current Eastern Time: {easternNow}===========================");
        Console.WriteLine($"============================Reviewed At: {dto.ReviewedAt}=============================");

        if (dto.ReviewedAt > easternNow)
            throw new ArgumentException("Reviewed At date cannot be in the future. It must be less than or equal to the current date (Eastern Time UTC-5).",
                            nameof(dto.ReviewedAt));


        var adverseEventIds = dto.ClinicalMedicalReviews
            .Select(x => x.AdverseEventId)
            .ToList();

        var adverseEvents = await _unitOfWork.GetRepository<AdverseEvent>()
                        .GetAllByItems(ad => ad.AefiReportId == medicalAssignment.AefiReportId
                                            && adverseEventIds.Contains(ad.Id))
                        .ToListAsync();

        if (adverseEvents.Count != adverseEventIds.Count)
        {
            throw new KeyNotFoundException("Some adverse events were not found or do not belong to the report.");
        }

        var adverseEventMap = adverseEvents.ToDictionary(x => x.Id);


        foreach (var clinical in dto.ClinicalMedicalReviews)
        {
            var adverseEvent = adverseEventMap[clinical.AdverseEventId];

            adverseEvent.LaboratoryResults = clinical.LaboratoryResults;
            adverseEvent.MedDRACode = clinical.MedDRACode;
            adverseEvent.RetClassification = clinical.RetClassification;

        }

        var medicalReview = _mapper.Map<MedicalReview>(dto);
        medicalReview.MedicalReviewAssignment = medicalAssignment;
        medicalReview.MedicalReviewAssignment.Status = ReviewAssignmentStatus.Completed;

        await _unitOfWork.GetRepository<MedicalReview>().CreateAsync(medicalReview);
        await _unitOfWork.CompleteAsync();

        return dto;
    }

    public async Task<MedicalReviewDto> UpdateAsync(MedicalReviewDto dto)
    {
        try
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Report DTO cannot be null.");

            // TODO: Implement update logic with proper validation
            await _unitOfWork.CompleteAsync();
            return dto;
        }
        catch (ArgumentNullException ex)
        {
            throw new InvalidOperationException($"Validation error: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while updating the report: {ex.Message}", ex);
        }
    }
    public async Task DeleteAsync<TId>(TId medicalReviewId)
    {
        try
        {
            if (medicalReviewId == null)
                throw new ArgumentException("Medical Review ID must be different than null.", nameof(medicalReviewId));

            var report = await _unitOfWork.GetRepository<MedicalReview>().GetByIdAsync(medicalReviewId);
            if (report == null)
                throw new KeyNotFoundException($"Medical Review with ID {medicalReviewId} does not exist.");

            await _unitOfWork.GetRepository<MedicalReview>().DeleteByIdAsync(medicalReviewId);
            await _unitOfWork.CompleteAsync();
        }
        catch (ArgumentException ex)
        {
            throw new InvalidOperationException($"Validation error: {ex.Message}", ex);
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while deleting the report: {ex.Message}", ex);
        }
    }
}