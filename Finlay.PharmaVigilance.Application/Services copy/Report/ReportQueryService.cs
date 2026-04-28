
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IServices.Common;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Application.Services;


public class ReportQueryService : GenericQueryService<AefiReport, PublicAefiReportDto>,
                                  IReportQueryService
{
    private static readonly Expression<Func<AefiReport, object>>[] includes =
                        { e => e.VaccinatedSubject,
                        e=> e.Reporter,
                         e=> e.Vaccinations,
                         e=> e.AdverseEvents
                        };

    private readonly IUserContextService _userContextService;

    public ReportQueryService(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
        : base(unitOfWork, mapper)
    {
        _userContextService = userContextService;
    }

    public override Expression<Func<AefiReport, object>>[] GetIncludes() => includes;



    public async Task<ReportDetailDto> GetReportByNotificationNumber(string notificationNumber)
    {
        var includes = GetIncludes();

        var report = await _unitOfWork.GetRepository<AefiReport>()
                        .FirstOrDefaultAsync(ar => ar.NotificationNumber == notificationNumber, default, includes);

        if (report?.VaccinatedSubject == null)
            Console.WriteLine("======================VaccinationSubject================");

        if (report?.Reporter == null)
            Console.WriteLine("======================Reporter================");

        if (report?.AdverseEvents == null)
            Console.WriteLine("======================AdverseEvent================");

        foreach (var i in report?.AdverseEvents!)
        {
            Console.WriteLine(i.CurrentStatus);
        }

        if (report?.Vaccinations == null)
            Console.WriteLine("======================Vaccinations================");


        foreach (var i in report?.Vaccinations!)
        {
            Console.WriteLine(i.DoseNumber);
        }
        // Console.WriteLine(result?.VaccinatedSubject);

        var vaccinatedSubject = await _unitOfWork.GetRepository<VaccinatedSubject>()
                                    .GetByIdAsync(report.VaccinatedSubjectId);

        var reporter = await _unitOfWork.GetRepository<Reporter>()
                            .GetByIdAsync(report.ReporterId);

        var vaccinations = await _unitOfWork.GetRepository<Vaccination>()
                        .GetAllByItems(v => v.AefiReportId == report.Id)
                        .ToListAsync();

        //var vaccinationList = vaccinations?.Select(_mapper.Map<VaccinationResponseDto>) ?? Enumerable.Empty<VaccinationResponseDto>();
        List<VaccinationResponseDto> vaccinationList = new List<VaccinationResponseDto>();

        for (int i = 0; i < vaccinations!.Count; i++)
        {
            var vaccine = await _unitOfWork.GetRepository<Vaccine>()
                            .FirstOrDefaultAsync(v => v.Id == vaccinations[i].VaccineId);

            if (vaccine == null)
                throw new ArgumentNullException("Vaccine not exists");

            vaccinationList.Add(_mapper.Map<VaccinationResponseDto>(vaccinations[i]));
            vaccinationList[i].VaccineName = vaccine.Name;
        }
        var adverseEvents = await _unitOfWork.GetRepository<AdverseEvent>()
                        .GetAllByItems(ar => ar.AefiReportId == report.Id)
                        .ToListAsync();

        // var listadverseEvents = adverseEvents?.Select(_mapper.Map<AdverseEventResponseDto>) ?? Enumerable.Empty<AdverseEventResponseDto>();

        List<AdverseEventDetailDto> adverseEventsList = new List<AdverseEventDetailDto>();

        for (int i = 0; i < adverseEvents!.Count(); i++)
        {
            var adverseEventsSymptoms = await _unitOfWork.GetRepository<AdverseEventSymptom>()
                                        .GetAllByItems(ae => ae.AdverseEventId == adverseEvents![i].Id)
                                        .ToListAsync();

            Console.WriteLine("899999999999");

            List<Symptom> symptoms = new List<Symptom>();

            foreach (var j in adverseEventsSymptoms)
            {
                var symptom = await _unitOfWork.GetRepository<Symptom>()
                                .FirstOrDefaultAsync(s => s.Id == j.SymptomId);
                if (symptom == null)
                    throw new ArgumentNullException("Symptom not exists");

                symptoms.Add(symptom);
            }

            Console.WriteLine("090909090909");

            adverseEventsList.Add(_mapper.Map<AdverseEventDetailDto>(adverseEvents[i]));
            adverseEventsList[i].Symptoms = symptoms?.Select(_mapper.Map<GetSymptomDto>) ?? Enumerable.Empty<GetSymptomDto>();


        }

        return new ReportDetailDto
        {
            ReportDate = report.ReportDate,
            VaccinatedSubject = _mapper.Map<VaccinatedSubjectResponseDto>(vaccinatedSubject),
            Reporter = _mapper.Map<ReporterResponseDto>(reporter),
            Vaccinations = vaccinationList,
            AdverseEvents = adverseEventsList
        };


    }


    public async Task<IEnumerable<ReportDetailDto>> GetReportAssigment()
    {
        var userId = _userContextService.GetUserId();

        var medicalReviewer = await _unitOfWork.GetRepository<MedicalReviewer>()
                                        .FirstOrDefaultAsync(sr => sr.UserId == userId);

        if (medicalReviewer == null)
            throw new UnauthorizedAccessException("User is not a medical reviewer");

        var medicalreviewassignments = await _unitOfWork.GetRepository<MedicalReviewAssignment>()
                                            .GetAllByItems(
                                                mra => mra.MedicalReviewerId == medicalReviewer.Id &&
                                                mra.Status == Domain.Enum.ReviewAssignmentStatus.Pending)
                                            .ToListAsync();

        List<ReportDetailDto> reportResponses = new List<ReportDetailDto>();

        foreach (var medicalReviewAssigment in medicalreviewassignments)
        {

            var includes = GetIncludes();

            var report = await _unitOfWork.GetRepository<AefiReport>()
                            .GetByIdAsync(medicalReviewAssigment.AefiReportId);

            if (report?.VaccinatedSubject == null)
                Console.WriteLine("======================VaccinationSubject================");

            if (report?.Reporter == null)
                Console.WriteLine("======================Reporter================");

            if (report?.AdverseEvents == null)
                Console.WriteLine("======================AdverseEvent================");

            foreach (var i in report?.AdverseEvents!)
            {
                Console.WriteLine(i.CurrentStatus);
            }

            if (report?.Vaccinations == null)
                Console.WriteLine("======================Vaccinations================");


            foreach (var i in report?.Vaccinations!)
            {
                Console.WriteLine(i.DoseNumber);
            }
            // Console.WriteLine(result?.VaccinatedSubject);

            var vaccinatedSubject = await _unitOfWork.GetRepository<VaccinatedSubject>()
                                        .GetByIdAsync(report.VaccinatedSubjectId);

            var reporter = await _unitOfWork.GetRepository<Reporter>()
                                .GetByIdAsync(report.ReporterId);

            var vaccinations = await _unitOfWork.GetRepository<Vaccination>()
                            .GetAllByItems(v => v.AefiReportId == report.Id)
                            .ToListAsync();

            //var vaccinationList = vaccinations?.Select(_mapper.Map<VaccinationResponseDto>) ?? Enumerable.Empty<VaccinationResponseDto>();
            List<VaccinationResponseDto> vaccinationList = new List<VaccinationResponseDto>();

            for (int i = 0; i < vaccinations!.Count; i++)
            {
                var vaccine = await _unitOfWork.GetRepository<Vaccine>()
                                .FirstOrDefaultAsync(v => v.Id == vaccinations[i].VaccineId);

                if (vaccine == null)
                    throw new ArgumentNullException("Vaccine not exists");

                vaccinationList.Add(_mapper.Map<VaccinationResponseDto>(vaccinations[i]));
                vaccinationList[i].VaccineName = vaccine.Name;
            }
            var adverseEvents = await _unitOfWork.GetRepository<AdverseEvent>()
                            .GetAllByItems(ar => ar.AefiReportId == report.Id)
                            .ToListAsync();

            // var listadverseEvents = adverseEvents?.Select(_mapper.Map<AdverseEventResponseDto>) ?? Enumerable.Empty<AdverseEventResponseDto>();

            List<AdverseEventDetailDto> adverseEventsList = new List<AdverseEventDetailDto>();

            for (int i = 0; i < adverseEvents!.Count(); i++)
            {
                var adverseEventsSymptoms = await _unitOfWork.GetRepository<AdverseEventSymptom>()
                                            .GetAllByItems(ae => ae.AdverseEventId == adverseEvents![i].Id)
                                            .ToListAsync();

                Console.WriteLine("899999999999");

                List<Symptom> symptoms = new List<Symptom>();

                foreach (var j in adverseEventsSymptoms)
                {
                    var symptom = await _unitOfWork.GetRepository<Symptom>()
                                    .FirstOrDefaultAsync(s => s.Id == j.SymptomId);
                    if (symptom == null)
                        throw new ArgumentNullException("Symptom not exists");

                    symptoms.Add(symptom);
                }

                Console.WriteLine("090909090909");

                adverseEventsList.Add(_mapper.Map<AdverseEventDetailDto>(adverseEvents[i]));
                adverseEventsList[i].Symptoms = symptoms?.Select(_mapper.Map<GetSymptomDto>) ?? Enumerable.Empty<GetSymptomDto>();


            }

            reportResponses.Add(new ReportDetailDto
            {
                ReportDate = report.ReportDate,
                VaccinatedSubject = _mapper.Map<VaccinatedSubjectResponseDto>(vaccinatedSubject),
                Reporter = _mapper.Map<ReporterResponseDto>(reporter),
                Vaccinations = vaccinationList,
                AdverseEvents = adverseEventsList
            });


        }

        return reportResponses;

    }

    public async Task<PagedResultDto<ReportSummaryDto>> GetReportsBySectionResponsible(PagedRequestDto paged)
    {
        var userId = _userContextService.GetUserId();

        var sectionResponsible = await _unitOfWork.GetRepository<SectionResponsible>()
                                        .FirstOrDefaultAsync(sr => sr.UserId == userId);

        if (sectionResponsible == null)
            throw new UnauthorizedAccessException("User is not a section responsible");

        var reportIds = _unitOfWork.GetRepository<Alert>()
                            .GetAllByItems(a => a.SectionResponsibleId == sectionResponsible.Id &&
                                a.AefiReport.Status == ReportStatus.Submitted)
                            .Select(a => a.AefiReportId)
                            .Distinct();

        var reportsQuery = _unitOfWork.GetRepository<AefiReport>()
                                .GetAllByItems(r => reportIds.Contains(r.Id))
                                .OrderByDescending(r => r.ReportDate);


        var totalItems = await reportsQuery.CountAsync();

        var items = await _unitOfWork.GetRepository<AefiReport>()
                        .GetPaged(reportsQuery, (paged.PageNumber - 1) * paged.PageSize, paged.PageSize)
                        .ProjectTo<ReportSummaryDto>(_mapper.ConfigurationProvider)
                        .ToListAsync();

        return new PagedResultDto<ReportSummaryDto>
        {
            Items = items,
            TotalCount = totalItems,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize,
            NextPageUrl = paged.PageNumber * paged.PageSize < totalItems
                        ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber + 1}&pageSize={paged.PageSize}"
                        : null,
            PreviousPageUrl = paged.PageNumber > 1
                        ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber - 1}&pageSize={paged.PageSize}"
                        : null

        };

    }


}