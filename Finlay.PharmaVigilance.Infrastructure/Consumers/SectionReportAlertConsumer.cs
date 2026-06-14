using Finlay.PharmaVigilance.Domain.Events;
using Finlay.PharmaVigilance.Application.IServices;
using MassTransit;
using Finlay.PharmaVigilance.Domain.Enum;
using Finlay.PharmaVigilance.Application.DTO;

namespace Finlay.PharmaVigilance.Infrastructure.Consumers;

public class SectionReportAlertConsumer : IConsumer<SectionReportAlertEvent>
{
    // private readonly IEmailService _emailService;
    private readonly IMessageService _messageService;

    public SectionReportAlertConsumer(
        IMessageService messageService)
    {
        _messageService = messageService;
    }

    public async Task Consume(ConsumeContext<SectionReportAlertEvent> context)
    {
        var data = context.Message;

        // await _emailService.SendEmailAsync(
        //     data.EmailSectionResponsible,
        //     EmailTemplateType.SectionReportAlert,
        //     new SectionReportAlertTemplate
        //     {
        //         NotificationNumber = data.ReportNumber
        //     }
        // );


        await _messageService.SendEmailAsync(
            data.PhoneNumber,
            EmailTemplateType.SectionReportAlert,
            new SectionReportAlertTemplate
            {
                NotificationNumber = data.ReportNumber
            }
        );
    }
}