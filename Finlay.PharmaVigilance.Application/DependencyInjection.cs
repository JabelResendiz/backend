using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.DTO.Authentication;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IServices.Authentication;
using Finlay.PharmaVigilance.Application.Services;
using Finlay.PharmaVigilance.Application.Services.Authentication;
using Finlay.PharmaVigilance.Application.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finlay.PharmaVigilance.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Adds application-specific services to the dependency injection container 
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>The modified IServiceCollection.</returns>
    public static IServiceCollection AddAplication(this IServiceCollection services, ConfigurationManager configurationManager)
    {

        // Registers AutoMapper to enable mapping between DTOs and domain models.
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // Email Service
        //services.AddScoped<IEmailAppService, EmailAppService>();

        // Captcha



        // Catalog Service
        services.AddScoped<ICatalogCommandService, CatalogCommandService>();
        services.AddScoped<IVaccineQueryService, VaccineQueryService>();
        services.AddScoped<ISymptomQueryService, SymptomQueryService>();

        // Registers services related to Entities
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IMedicalReviewerService, MedicalReviewerService>();
        services.AddScoped<ISectionResponsibleService, SectionResponsibleService>();

        // User Services
        services.AddScoped<IUserQueryServices, UserQueryService>();
        services.AddScoped<IUserCommandServices, UserCommandService>();

        // reporter
        services.AddScoped<IReportCommandService, ReportCommandService>();
        services.AddScoped<IReportQueryService, ReportQueryService>();

        // Report Validators - Chain of Responsibility pattern for comprehensive validation
        services.AddScoped<IReportValidator<ReportDto>, ReportDateValidator>();
        services.AddScoped<IReportValidator<PublicAefiReportDto>, ReporterValidator>();
        services.AddScoped<IReportValidator<ReportDto>, VaccinatedSubjectValidator>();
        services.AddScoped<IReportValidator<ReportDto>, VaccinationValidator>();
        services.AddScoped<IReportValidator<ReportDto>, AdverseEventValidator>();
        services.AddScoped<IReportValidator<RegisterMedicalReviewerDto>, MedicalReviewerValidator>();

        // Notification Number Generator
        services.AddScoped<INotificationNumberGenerator, NotificationNumberGenerator>();

        // Medical review
        services.AddScoped<IMedicalReviewCommandService, MedicalReviewCommandService>();
        services.AddScoped<IMedicalReviewQueryService, MedicalReviewQueryService>();

        services.AddScoped<IMedicalReviewAssignmentCommandService, MedicalReviewAssignmentCommandService>();

        services.AddScoped<IVaccinationCenterCommandService, VaccinationCenterCommandService>();
        services.AddScoped<IVaccinationCenterQueryService, VaccinationCenterQueryService>();


        services.AddScoped<ILotCommandService, LotCommandService>();
        services.AddScoped<ILotQueryService, LotQueryService>();

        services.AddScoped<IManufacturerQueryService, ManufacturerQueryService>();

        services.AddScoped<IMunicipalDashboardService, MunicipalDashboardService>();

        return services;


    }
}