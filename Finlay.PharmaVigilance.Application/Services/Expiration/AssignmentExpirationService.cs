using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;
using Finlay.PharmaVigilance.Domain.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Application.Services;


public class AssignmentExpirationService
    : IAssignmentExpirationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;


    public AssignmentExpirationService(
        IUnitOfWork unitOfWork,
        IPublishEndpoint publishEndpoint)
    {
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }

    public async Task ProcessExpiredAssignmentsAsync(
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        //var expirationLimit = now.AddMinutes(-1);
        var expirationLimit = now.AddDays(-7);

        var assignments = await _unitOfWork
            .GetRepository<MedicalReviewAssignment>()
            .GetAllByItems(a =>
                a.Status == ReviewAssignmentStatus.Pending &&
                a.AssignedAt <= expirationLimit)
            .Select(a => new
            {
                Assignment = a,
                Report = a.AefiReport,
                Email = a.SectionResponsible.User.Email
            })
            .ToListAsync();

        foreach (var item in assignments)
        {
            item.Assignment.Status = ReviewAssignmentStatus.Expired;

            if (item.Report != null)
            {
                item.Report.Status = ReportStatus.Reopened;
            }
        }

        await _unitOfWork.CompleteAsync(cancellationToken);

        foreach (var item in assignments)
        {

            await _publishEndpoint.Publish(
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