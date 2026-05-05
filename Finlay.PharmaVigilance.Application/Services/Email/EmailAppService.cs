using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.Services;


public class EmailAppService : IEmailAppService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public EmailAppService(IUnitOfWork unitOfWork, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task SendEmailToSectionResponsibleAsync(SectionResponsible sectionResponsible)
    {

        var user = await _unitOfWork.UserRepository
                            .GetByIdAsync(sectionResponsible.UserId)
                            ?? throw new ArgumentException($"User with ID {sectionResponsible.UserId} dont founded");

        var email = user.Email ?? throw new ArgumentException("Null Email");

        await _emailService.SendEmailAsync(email,
            "Nueva Alerta de Reporte Creado",
            "Tiene nueva alerta de reporte creado en el sistema de autorreporte del Instituo Finlay.Acceda al portal de https://..../..../ para poder asignar el reporte");

    }

    public async Task SendEmailToReporterAsync(Reporter reporter)
    {
        var email = reporter.Email ?? throw new ArgumentException("Null Email");

        await _emailService.SendEmailAsync(email,
            "Nueva Alerta de Reporte Creado",
            "Tiene nueva alerta de reporte creado en el sistema de autorreporte del Instituo Finlay.Acceda al portal de https://..../..../ para poder asignar el reporte");

    }

    public async Task SendEmailToMedicalReviewerAsync(MedicalReviewer medicalReviewer)
    {
        var user = await _unitOfWork.UserRepository
                            .GetByIdAsync(medicalReviewer.UserId)
                            ?? throw new ArgumentException($"User with ID {medicalReviewer.UserId} dont founded");

        var email = user.Email ?? throw new ArgumentException("Null Email");

        await _emailService.SendEmailAsync(email,
            "Nueva Alerta de Reporte Creado",
            "Tiene nueva alerta de reporte creado en el sistema de autorreporte del Instituo Finlay.Acceda al portal de https://..../..../ para poder asignar el reporte");

    }
}