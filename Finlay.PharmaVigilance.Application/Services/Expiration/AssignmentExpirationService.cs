using Finlay.PharmaVigilance.Application.Common.EventBus;
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
        var now = DateTime.UtcNow;

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


        foreach (var item in expired)
        {
            item.Assignment.Status = ReviewAssignmentStatus.Expired;

            if (item.Report != null)
            {
                item.Report.Status = ReportStatus.Reopened;
            }
        }

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


    // public async Task ProcessExpiredAssignmentsAsync(CancellationToken cancellationToken = default)
    // {
    //     var now = DateTime.UtcNow;

    //     // 1. Ejecutar actualización masiva directamente en la BD (ExecuteUpdateAsync)
    //     // Esto no trae filas a memoria, se ejecuta como un solo query SQL UPDATE instantáneo.
    //     var affectedRows = await _unitOfWork.GetRepository<MedicalReviewAssignment>()
    //         .GetAll() // Necesitas el IQueryable directo del repositorio
    //         .Where(a => a.Status == ReviewAssignmentStatus.Pending &&
    //             (
    //                 (a.AefiReport.Priority == ReportPriority.High && a.AssignedAt.AddHours(24) < now) ||
    //                 (a.AefiReport.Priority == ReportPriority.Medium && a.AssignedAt.AddDays(5) < now) ||
    //                 (a.AefiReport.Priority != ReportPriority.High && a.AefiReport.Priority != ReportPriority.Medium && a.AssignedAt.AddDays(7) < now)
    //             ))
    //         .ExecuteUpdateAsync(setters => setters
    //             .SetProperty(a => a.Status, ReviewAssignmentStatus.Expired)
    //             .SetProperty(a => a.AefiReport.Status, ReportStatus.Reopened),
    //             cancellationToken);

    //     // Si no hubo expirados, terminamos inmediatamente sin consultar nada más
    //     if (affectedRows == 0) return;

    //     // 2. Obtener SOLO los datos necesarios para los eventos de los registros ya expirados
    //     var expiredEventsData = await _unitOfWork.GetRepository<MedicalReviewAssignment>()
    //         .GetAll()
    //         .Where(a => a.Status == ReviewAssignmentStatus.Expired && a.AefiReport.Status == ReportStatus.Reopened)
    //         .Select(a => new AssignmentExpiredEvent
    //         {
    //             AssignmentId = a.Id,
    //             ReportId = a.AefiReport.Id,
    //             SectionResponsibleEmail = a.SectionResponsible.User.Email!
    //         })
    //         .ToListAsync(cancellationToken);

    //     // 3. Publicar los eventos en paralelo
    //     var publishTasks = expiredEventsData.Select(ev => _eventBus.PublishAsync(ev, cancellationToken));
    //     await Task.WhenAll(publishTasks);
    // }
}