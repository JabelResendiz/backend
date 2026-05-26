using Finlay.PharmaVigilance.Domain.Events;
using Finlay.PharmaVigilance.Application.IServices;
using MassTransit;
using Finlay.PharmaVigilance.Domain.Enum;
using Finlay.PharmaVigilance.Application.DTO;

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
            EmailTemplateType.AssignmentExpired,
            new AssignmentExpiredTemplate
            {
                ReportId = data.ReportId
            }
        );
    }
}