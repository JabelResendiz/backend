using AutoMapper;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IServices.Common;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Application.Helpers;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;
using Finlay.PharmaVigilance.Domain.Events;
using Finlay.PharmaVigilance.Application.Common.EventBus;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Application.Services;

public class MedicalReviewAssignmentCommandService : IMedicalReviewAssignmentCommandService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;
    private readonly IEventBus _eventBus;

    public MedicalReviewAssignmentCommandService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IUserContextService userContextService,
        IEventBus eventBus
    )
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork)); ;
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); ;
        _userContextService = userContextService ?? throw new ArgumentNullException(nameof(userContextService));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public async Task<MedicalReviewAssignmentDTO> CreateAsync(MedicalReviewAssignmentDTO dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var userId = _userContextService.GetUserId();

        var sectionResponsible = await _unitOfWork.GetRepository<SectionResponsible>()
                                        .FirstOrDefaultAsync(sr => sr.UserId == userId);

        if (sectionResponsible == null)
            throw new UnauthorizedAccessException("User is not a section responsible.");

        var report = await _unitOfWork.GetRepository<AefiReport>()
        .FirstOrDefaultAsync(d => d.Id == dto.AefiReportId && d.Status != ReportStatus.Draft);
        // .GetByIdAsync(dto.AefiReportId);

        if (report == null)
            throw new KeyNotFoundException("Aefi Report not found.");

        var existReport = await _unitOfWork.GetRepository<MedicalReviewAssignment>()
                                .FirstOrDefaultAsync(mra => mra.AefiReportId == dto.AefiReportId
                                && mra.Status != ReviewAssignmentStatus.Expired);

        if (existReport != null)
            throw new InvalidOperationException("This report is already assigned to a medical reviewer.");

        var medicalReviewer = await _unitOfWork.GetRepository<MedicalReviewer>()
                                    .GetByIdAsync(dto.MedicalReviewerId, default, o => o.User);

        if (medicalReviewer == null)
            throw new KeyNotFoundException("Medical Reviewer not found.");

        var alert = await _unitOfWork.GetRepository<Alert>()
                                .FirstOrDefaultAsync(a => a.AefiReportId == report.Id);

        if (alert == null)
            throw new KeyNotFoundException("Alert not found.");

        if (alert.SectionResponsibleId != sectionResponsible.Id)
            throw new UnauthorizedAccessException("Section Responsible does not have permission to assign this report.");


        if (medicalReviewer.MunicipalityId != sectionResponsible.MunicipalityId)
            throw new InvalidOperationException(
                "Medical Reviewer must be from the same municipality as the Section Responsible.");

        var easternNow = TimeZoneHelper.GetEasternNow();
        if (dto.AssignedAt > easternNow)
            throw new ArgumentException("Assigned At date cannot be in the future. It must be less than or equal to the current date (Eastern Time UTC-5).",
                            nameof(dto.AssignedAt));

        if (dto.AssignedAt < report.ReportDate)
            throw new ArgumentException("Assigned At date cannot be before the report creation date.",
                            nameof(dto.AssignedAt));

        var medicalReviewAssignment = _mapper.Map<MedicalReviewAssignment>(dto);
        medicalReviewAssignment.SectionResponsible = sectionResponsible;
        medicalReviewAssignment.MedicalReviewer = medicalReviewer;
        medicalReviewAssignment.AefiReport = report;
        medicalReviewAssignment.SectionResponsibleId = sectionResponsible.Id;


        medicalReviewAssignment.Status = ReviewAssignmentStatus.Pending;
        report.Status = ReportStatus.UnderReview;

        await _unitOfWork
        .GetRepository<MedicalReviewAssignment>()
        .CreateAsync(medicalReviewAssignment);
        await _unitOfWork.CompleteAsync();


        // await _eventBus.PublishAsync(new NewAssignmentEvent
        // {
        //     MedicalReviewerName = medicalReviewer.User.UserName!,
        //     MedicalReviewerEmail = medicalReviewer.User.Email!,
        //     ReportNumber = report.NotificationNumber
        // });

        return dto;
    }


    public async Task ReassignedAsync(MedicalReviewAssignmentDTO dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var userId = _userContextService.GetUserId();

        var sectionResponsible = await _unitOfWork.GetRepository<SectionResponsible>()
                                        .FirstOrDefaultAsync(sr => sr.UserId == userId);

        if (sectionResponsible == null)
            throw new UnauthorizedAccessException("User is not a section responsible.");

        var report = await _unitOfWork.GetRepository<AefiReport>()
                                .GetByIdAsync(dto.AefiReportId);

        if (report == null)
            throw new KeyNotFoundException("Aefi Report not found.");

        var existReport = await _unitOfWork.GetRepository<MedicalReviewAssignment>()
                                .FirstOrDefaultAsync(mra => mra.AefiReportId == dto.AefiReportId
                                && mra.Status == ReviewAssignmentStatus.Completed);

        if (existReport != null)
            throw new InvalidOperationException("This report is done.");

        var medicalReviewer = await _unitOfWork.GetRepository<MedicalReviewer>()
                                    .GetByIdAsync(dto.MedicalReviewerId, default, o => o.User);

        if (medicalReviewer == null)
            throw new KeyNotFoundException("Medical Reviewer not found.");

        var alert = await _unitOfWork.GetRepository<Alert>()
                                .FirstOrDefaultAsync(a => a.AefiReportId == report.Id);

        if (alert == null)
            throw new KeyNotFoundException("Alert not found.");

        if (alert.SectionResponsibleId != sectionResponsible.Id)
            throw new UnauthorizedAccessException("Section Responsible does not have permission to assign this report.");


        if (medicalReviewer.MunicipalityId != sectionResponsible.MunicipalityId)
            throw new InvalidOperationException(
                "Medical Reviewer must be from the same municipality as the Section Responsible.");

        var easternNow = TimeZoneHelper.GetEasternNow();
        if (dto.AssignedAt > easternNow)
            throw new ArgumentException("Assigned At date cannot be in the future. It must be less than or equal to the current date (Eastern Time UTC-5).",
                            nameof(dto.AssignedAt));

        if (dto.AssignedAt < report.ReportDate)
            throw new ArgumentException("Assigned At date cannot be before the report creation date.",
                            nameof(dto.AssignedAt));

        var newMedicalReviewAssignment = _mapper.Map<MedicalReviewAssignment>(dto);


        var oldMedicalReviewAssignment = await _unitOfWork.GetRepository<MedicalReviewAssignment>()
                                        .FirstOrDefaultAsync(mr => mr.AefiReportId == dto.AefiReportId &&
                                        mr.Status == ReviewAssignmentStatus.Pending)
                                        ?? throw new ArgumentException("Reassign failed");

        newMedicalReviewAssignment.SectionResponsible = sectionResponsible;
        newMedicalReviewAssignment.MedicalReviewer = medicalReviewer;
        newMedicalReviewAssignment.AefiReport = report;
        newMedicalReviewAssignment.SectionResponsibleId = sectionResponsible.Id;
        newMedicalReviewAssignment.Status = ReviewAssignmentStatus.Pending;
        oldMedicalReviewAssignment.Status = ReviewAssignmentStatus.Cancelled;
        report.Status = ReportStatus.UnderReview;

        try
        {
            await _unitOfWork.GetRepository<MedicalReviewAssignment>().CreateAsync(newMedicalReviewAssignment);

            await _unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new InvalidOperationException(
            "The assignment was modified by another user while processing the reassignment. " +
            "The reviewer may have completed the evaluation. Please refresh and verify.");
        }



        // await _eventBus.PublishAsync(new NewAssignmentEvent
        // {
        //     MedicalReviewerName = medicalReviewer.User.UserName!,
        //     MedicalReviewerEmail = medicalReviewer.User.Email!,
        //     ReportNumber = report.NotificationNumber
        // });

    }

    public async Task<MedicalReviewAssignmentDTO> UpdateAsync(MedicalReviewAssignmentDTO dto)
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
    public async Task DeleteAsync<TId>(TId assignmentId)
    {
        try
        {
            if (assignmentId == null)
                throw new ArgumentException("Medical Review Assignment ID must be different than null.", nameof(assignmentId));

            var assignment = await _unitOfWork.GetRepository<MedicalReviewAssignment>().GetByIdAsync(assignmentId);

            if (assignment == null)
                throw new KeyNotFoundException($"Medical Review Assignment with ID {assignmentId} does not exist.");

            var report = await _unitOfWork.GetRepository<AefiReport>().GetByIdAsync(assignment.AefiReportId);

            report.Status = ReportStatus.Submitted;

            await _unitOfWork.GetRepository<MedicalReviewAssignment>().DeleteByIdAsync(assignmentId);
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