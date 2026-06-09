using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Domain.Enum;
using Finlay.PharmaVigilance.Domain.Events;
using MassTransit;


namespace Finlay.PharmaVigilance.Infrastructure.Consumers;

public class NewAssignmentConsumer : IConsumer<NewAssignmentEvent>
{
    private readonly IEmailService _emailService;

    public NewAssignmentConsumer(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Consume(ConsumeContext<NewAssignmentEvent> context)
    {
        var data = context.Message;

        await _emailService.SendEmailAsync(
            data.MedicalReviewerEmail!,
            EmailTemplateType.MedicalReviewerAssignment,
            new NewAssignmentTemplate
            {
                NotificationNumber = data.ReportNumber
            }
        );

    }

}