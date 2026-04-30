using System.Linq.Expressions;
using AutoMapper;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IServices.Common;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Application.Helpers;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.Services;

public class MedicalReviewAssignmentCommandService : IMedicalReviewAssignmentCommandService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;
    private readonly IEmailAppService _emailAppService;

    public MedicalReviewAssignmentCommandService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IUserContextService userContextService,
        IEmailAppService emailAppService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork)); ;
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); ;
        _userContextService = userContextService ?? throw new ArgumentNullException(nameof(userContextService));
        _emailAppService = emailAppService ?? throw new ArgumentNullException(nameof(emailAppService));
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
                                .GetByIdAsync(dto.AefiReportId);

        if (report == null)
            throw new KeyNotFoundException("Aefi Report not found.");

        var existReport = await _unitOfWork.GetRepository<MedicalReviewAssignment>()
                                .FirstOrDefaultAsync(mra => mra.AefiReportId == dto.AefiReportId
                                && mra.Status != ReviewAssignmentStatus.Expired);

        if (existReport != null)
            throw new InvalidOperationException("This report is already assigned to a medical reviewer.");

        var medicalReviewer = await _unitOfWork.GetRepository<MedicalReviewer>()
                                    .GetByIdAsync(dto.MedicalReviewerId);
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

        Console.WriteLine($"========================dto.AssignedAt : {dto.AssignedAt}=======================");
        Console.WriteLine($"========================report.ReportDate : {report.ReportDate}=======================");

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

        await _unitOfWork.GetRepository<MedicalReviewAssignment>().CreateAsync(medicalReviewAssignment);
        await _unitOfWork.CompleteAsync();

        await _emailAppService.SendEmailToMedicalReviewerAsync(medicalReviewer);

        return dto;
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