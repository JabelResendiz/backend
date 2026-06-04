using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;
using MassTransit;
using Microsoft.EntityFrameworkCore;


namespace Finlay.PharmaVigilance.Infrastructure.Consumers;

public class ReportConfirmationConsumer : IConsumer<ReportConfirmationEvent>
{
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;

    public ReportConfirmationConsumer(IEmailService emailService, IUnitOfWork unitOfWork)
    {
        _emailService = emailService;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<ReportConfirmationEvent> context)
    {
        var data = context.Message;

        var url = "http://localhost:5173";


        var symptomNames = await _unitOfWork
           .GetRepository<Symptom>()
           .GetAllByItems(s => data.SymptomIds.Contains(s.Id))
           .Select(s => s.Name)
           .ToListAsync();

        var vaccineNames = await _unitOfWork
            .GetRepository<Vaccine>()
            .GetAllByItems(v => data.VaccineIds.Contains(v.Id))
            .Select(v => v.Name)
            .ToListAsync();

        await _emailService.SendEmailAsync(
            data.Email!,
            EmailTemplateType.SelfReportConfirmation,
            new ReportConfirmationTemplate
            {
                Vaccines = string.Join(",", vaccineNames),
                Symptoms = string.Join(",", symptomNames),
                ReportDate = data.ReportDate.ToString(),
                NotificationNumber = data.ReportNumber,
                Url = url
            }
        );

    }

}