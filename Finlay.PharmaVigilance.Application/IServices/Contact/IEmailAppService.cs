using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IEmailAppService
{
    Task SendEmailToSectionResponsibleAsync(SectionResponsible user);
    Task SendEmailToReporterAsync(Reporter reporter);
    Task SendEmailToMedicalReviewerAsync(MedicalReviewer medicalReviewer);

}