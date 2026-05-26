using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Domain.Enum;
using Finlay.PharmaVigilance.Domain.Events;
using MassTransit;


namespace Finlay.PharmaVigilance.Infrastructure.Consumers;

public class ReportConfirmationConsumer : IConsumer<ReportConfirmationEvent>
{
    private readonly IEmailService _emailService;

    public ReportConfirmationConsumer(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Consume(ConsumeContext<ReportConfirmationEvent> context)
    {
        var data = context.Message;

        var url = "http://localhost:5173";

        await _emailService.SendEmailAsync(
            data.Email!,
            EmailTemplateType.SelfReportConfirmation,
            new ReportConfirmationTemplate
            {
                Vaccines = string.Join(",", data.VaccinesName),
                Symptoms = string.Join(",", data.SymptomsName),
                ReportDate = data.ReportDate.ToString(),
                NotificationNumber = data.ReportNumber,
                Url = url
            }
        );

    }

}