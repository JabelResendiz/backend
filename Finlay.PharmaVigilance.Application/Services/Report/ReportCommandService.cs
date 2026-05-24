using System.Linq.Expressions;
using AutoMapper;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IServices.Common;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Application.Validators;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;
using Finlay.PharmaVigilance.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Finlay.PharmaVigilance.Application.Services;

/// <summary>
/// Service for managing report command operations (Create, Update, Delete).
/// Handles creation, update, and deletion of AEFI (Adverse Event Following Immunization) reports
/// with comprehensive validation and error handling.
/// 
/// This service uses the Chain of Responsibility pattern with IReportValidator implementations
/// to ensure all business rules are validated before creating a report.
/// </summary>
public class ReportCommandService : IReportCommandService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly INotificationNumberGenerator _generator;
    private readonly IEnumerable<IReportValidator<ReportDto>> _validators;
    private readonly IEnumerable<IReportValidator<PublicAefiReportDto>> _publicValidators;
    private readonly IUserContextService _userContextService;
    private readonly ILogger<ReportCommandService> _logger;

    private static readonly Expression<Func<MedicalReviewer, object>>[] includes =
                            { e => e.User! };

    public ReportCommandService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        INotificationNumberGenerator generator,
        IEnumerable<IReportValidator<ReportDto>> validators,
        IEnumerable<IReportValidator<PublicAefiReportDto>> publicValidators,
        IUserContextService userContextService,
        ILogger<ReportCommandService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _generator = generator ?? throw new ArgumentNullException(nameof(generator));
        _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        _publicValidators = publicValidators ?? throw new ArgumentNullException(nameof(publicValidators));
        _userContextService = userContextService ?? throw new ArgumentNullException(nameof(userContextService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Expression<Func<MedicalReviewer, object>>[] GetIncludes() => includes;


    public async Task<CreateReportResponseDto> CreatePublicReportAsync(PublicAefiReportDto reportDto)
    {

        _logger.LogInformation("Starting public AEFI report creation process");

        if (reportDto == null)
            throw new ArgumentNullException(nameof(reportDto), "Report data is required.");

        try
        {

            _logger.LogDebug("Executing {ValidatorCount} validators", _validators.Count());

            // Execute all validators in sequence using Chain of Responsibility pattern
            foreach (var validator in _validators)
            {

                await validator.ValidateAsync(reportDto);
            }

            _logger.LogDebug("Executing {ValidatorCount} validators", _publicValidators.Count());

            foreach (var validator in _publicValidators)
            {

                await validator.ValidateAsync(reportDto);
            }

            // Step 1: Get or create VaccinatedSubject (patient)
            var vaccinatedSubjectRepository = _unitOfWork.GetRepository<VaccinatedSubject>();
            var existingVaccinatedSubject = await vaccinatedSubjectRepository
                .FirstOrDefaultAsync(x => x.IdentityNumber.Value == reportDto.VaccinatedSubject.IdentityNumber);

            VaccinatedSubject vaccinatedSubject;
            if (existingVaccinatedSubject != null)
            {
                _logger.LogDebug("Existing vaccinated subject found with IdentityNumber");

                vaccinatedSubject = existingVaccinatedSubject;
            }
            else
            {
                _logger.LogDebug("Creating new vaccinated subject");

                vaccinatedSubject = _mapper.Map<VaccinatedSubject>(reportDto.VaccinatedSubject);
            }

            // Step 2: Get or create Reporter by normalized full name
            var reporterRepository = _unitOfWork.GetRepository<Reporter>();
            var existingReporter = await reporterRepository
                .FirstOrDefaultAsync(x => x.IdentityNumber.Value == reportDto.Reporter.IdentityNumber);

            Reporter reporter;
            if (existingReporter != null)
            {
                _logger.LogDebug("Existing reporter found with IdentityNumber");
                reporter = existingReporter;
            }
            else
            {
                _logger.LogDebug("Creating new reporter");

                reporter = _mapper.Map<Reporter>(reportDto.Reporter);
            }

            var report = _mapper.Map<AefiReport>(reportDto);
            report.VaccinatedSubjectId = vaccinatedSubject.Id;
            report.VaccinatedSubject = vaccinatedSubject;
            report.ReporterId = reporter.Id;
            report.Reporter = reporter;
            report.Status = ReportStatus.Submitted;
            report.NotificationNumber = _generator.Generate();


            var sectionResponsible = await _unitOfWork.GetRepository<SectionResponsible>()
                .FirstOrDefaultAsync(sr => sr.MunicipalityId == reportDto.VaccinatedSubject.MunicipalityId);

            if (sectionResponsible == null)
            {
                _logger.LogWarning("No SectionResponsible found for MunicipalityId {MunicipalityId}", reportDto.VaccinatedSubject.MunicipalityId);
                throw new InvalidOperationException("No SectionResponsible found.");
            }
            // throw new InvalidOperationException("No SectionResponsible found.");

            var alert = new Alert
            {
                Description = "New AEFI report submitted",
                IsActive = true,
                IsRead = false,
                ReadAt = null,
                SectionResponsibleId = sectionResponsible.Id
            };

            report.Alerts.Add(alert);

            _logger.LogInformation("Saving AEFI report with NotificationNumber {NotificationNumber}", report.NotificationNumber);

            await _unitOfWork.GetRepository<AefiReport>().CreateAsync(report);
            await _unitOfWork.CompleteAsync();

            // send email

            _logger.LogDebug("Preparing to send notification emails for report");

            if (reporter.Email == null)
                throw new InvalidOperationException("Reporter email is null.");

            var sectionResponsibleUser = await _unitOfWork.UserRepository
                        .GetByIdAsync(sectionResponsible.UserId);

            if (sectionResponsibleUser == null)
                throw new InvalidOperationException("Section responsible user is null.");

            _logger.LogDebug($"📧 Queremos enviar email a: {reporter.Email} y {sectionResponsibleUser.Email}");

            // await _eventBus.PublishAsync(new EmailToReporterEvent
            // {
            //     ReportNumber = report.NotificationNumber,
            //     ReporterEmail = reporter.Email
            // });

            // await _eventBus.PublishAsync(new EmailToSectionResponsibleEvent
            // {
            //     ReportNumber = report.NotificationNumber,
            //     SectionResponsibleEmail = sectionResponsibleUser.Email!
            // });


            return new CreateReportResponseDto
            {
                NotificationNumber = report.NotificationNumber
            };

        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "Error creating AEFI report");


            throw new InvalidOperationException(
                $"Error to create AEFI report: {ex.Message}",
                ex);
        }
    }

    public async Task<CreateReportResponseDto> CreateMedicalReportAsync(MedicalReportDto reportDto)
    {
        if (reportDto == null)
            throw new ArgumentNullException(nameof(reportDto), "Medical report data is required.");

        // TODO: Implement medical report creation with validation
        // Medical reports may have different validation rules than public reports

        try
        {

            // Execute all validators in sequence using Chain of Responsibility pattern
            foreach (var validator in _validators)
            {
                await validator.ValidateAsync(reportDto);
            }

            // Step 1: Get or create VaccinatedSubject (patient)
            var vaccinatedSubjectRepository = _unitOfWork.GetRepository<VaccinatedSubject>();
            var existingVaccinatedSubject = await vaccinatedSubjectRepository
                .FirstOrDefaultAsync(x => x.IdentityNumber.Value == reportDto.VaccinatedSubject.IdentityNumber);

            VaccinatedSubject vaccinatedSubject;
            if (existingVaccinatedSubject != null)
            {
                vaccinatedSubject = existingVaccinatedSubject;
            }
            else
            {
                vaccinatedSubject = _mapper.Map<VaccinatedSubject>(reportDto.VaccinatedSubject);
            }

            // Step 2: Get or create Reporter by normalized full name

            var userId = _userContextService.GetUserId();

            var includes = GetIncludes();

            var medicalReviewer = await _unitOfWork.GetRepository<MedicalReviewer>()
                                        .FirstOrDefaultAsync(mr => mr.UserId == userId, default, includes);


            if (medicalReviewer == null)
                throw new UnauthorizedAccessException("User is not a medicalReviewer.");

            var reporterRepository = _unitOfWork.GetRepository<Reporter>();
            var existingReporter = await reporterRepository
                .FirstOrDefaultAsync(x => x.IdentityNumber == medicalReviewer.IdentityNumber);

            Reporter reporter;
            if (existingReporter != null)
            {
                reporter = existingReporter;
            }
            else
            {
                reporter = _mapper.Map<Reporter>(medicalReviewer);
                reporter.ReporterRelationship = ReporterRelationship.Doctor;
                reporter.UserId = medicalReviewer.UserId;

            }


            // Step 3
            var report = _mapper.Map<AefiReport>(reportDto);
            report.VaccinatedSubjectId = vaccinatedSubject.Id;
            report.VaccinatedSubject = vaccinatedSubject;

            //var reporter = _mapper.Map<Reporter>(medicalReviewer);
            // reporter.ReporterRelationship = ReporterRelationship.Doctor;
            // reporter.UserId = medicalReviewer.UserId;
            report.Reporter = reporter;
            report.ReporterId = reporter.Id;


            report.Status = ReportStatus.UnderReview;
            report.isMedicalReport = true;
            report.NotificationNumber = _generator.Generate();

            var sectionResponsible = await _unitOfWork.GetRepository<SectionResponsible>()
                .FirstOrDefaultAsync(sr => sr.MunicipalityId == reportDto.VaccinatedSubject.MunicipalityId);

            if (sectionResponsible == null)
                throw new InvalidOperationException("No SectionResponsible found.");

            var alert = new Alert
            {
                Description = "New AEFI report submitted",
                IsActive = true,
                IsRead = false,
                ReadAt = null,
                SectionResponsibleId = sectionResponsible.Id
            };

            report.Alerts.Add(alert);

            await _unitOfWork.GetRepository<AefiReport>().CreateAsync(report);
            await _unitOfWork.CompleteAsync();

            return new CreateReportResponseDto
            {
                NotificationNumber = report.NotificationNumber
            };

        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Error to create AEFI report: {ex.Message}",
                ex);
        }

        //return Task.FromResult(string.Empty);
    }

}