using Finlay.PharmaVigilance.Domain.Events;
using Finlay.PharmaVigilance.Application.IServices;
using MassTransit;

namespace Finlay.PharmaVigilance.Infrastructure.Consumers;

public class AssignmentExpiredConsumer : IConsumer<AssignmentExpiredEvent>
{
    private readonly IEmailService _emailService;

    public AssignmentExpiredConsumer(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Consume(ConsumeContext<AssignmentExpiredEvent> context)
    {
        var data = context.Message;

        await _emailService.SendEmailAsync(
            data.SectionResponsibleEmail,
            "Asignación expirada",
            $"El reporte {data.ReportId} ha expirado y ha sido reabierto.");
    }
}