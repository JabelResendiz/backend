using Finlay.PharmaVigilance.Application.Common.EventBus;
using Finlay.PharmaVigilance.Application.Helpers;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;
using Finlay.PharmaVigilance.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Application.Services;


public class AssignmentExpirationService
    : IAssignmentExpirationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;


    public AssignmentExpirationService(
        IUnitOfWork unitOfWork,
        IEventBus eventBus)
    {
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
    }

    public async Task ProcessExpiredAssignmentsAsync(
        CancellationToken cancellationToken = default)
    {
        var now = TimeZoneHelper.GetEasternNow();

        var allPending = await _unitOfWork
            .GetRepository<MedicalReviewAssignment>()
            .GetAllByItems(a => a.Status == ReviewAssignmentStatus.Pending)
            .Select(a => new
            {
                Assignment = a,
                Report = a.AefiReport,
                Email = a.SectionResponsible.User.Email
            })
            .ToListAsync(cancellationToken);

        var expired = allPending
        .Where(item =>
        {
            var deadline = item.Report.Priority switch
            {
                ReportPriority.High => item.Assignment.AssignedAt.AddHours(24),
                ReportPriority.Medium => item.Assignment.AssignedAt.AddDays(5),
                _ => item.Assignment.AssignedAt.AddDays(7)
            };
            return now > deadline;
        })
        .ToList();

        // foreach (var item in expired)
        // {
        //     item.Assignment.Status = ReviewAssignmentStatus.Expired;

        //     if (item.Report != null)
        //     {
        //         item.Report.Status = ReportStatus.Reopened;
        //     }
        // }

        await _unitOfWork.CompleteAsync(cancellationToken);

        foreach (var item in expired)
        {

            await _eventBus.PublishAsync(
                new AssignmentExpiredEvent
                {
                    AssignmentId = item.Assignment.Id,
                    ReportId = item.Report.Id,
                    SectionResponsibleEmail = item.Email!
                },
                cancellationToken);
        }
    }

}