
using AutoMapper;
using Finlay.PharmaVigilance.Application.DTO.Authentication;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.DTO;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<RegisterUserDto, User>();

        CreateMap<User, UserResponseDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.UserRole));


        CreateMap<User, GetUserDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.UserRole));

        // Medical Reviewer Registration
        CreateMap<RegisterMedicalReviewerDto, MedicalReviewer>();
        CreateMap<MedicalReviewer, GetMedicalReviewerDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.UserName));

        CreateMap<RegisterMedicalReviewerDto, User>();

        // Section Responsible Registration
        CreateMap<RegisterSectionResponsibleDto, SectionResponsible>();
        CreateMap<RegisterSectionResponsibleDto, User>();
        CreateMap<SectionResponsible, SectionResponsibleResponseDto>()
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));

        // Catalog Dtos
        CreateMap<VaccineDto, Vaccine>();
        CreateMap<Vaccine, GetVaccineDto>();
        CreateMap<Vaccine, GetPrivateVaccineDto>();
        CreateMap<Vaccine, VaccineDashboardDto>();

        CreateMap<SymptomDto, Symptom>();
        CreateMap<Symptom, GetSymptomDto>();
        CreateMap<Symptom, GetPrivateSymptomsDto>();

        // Manufacturer
        CreateMap<ManufacturerDto, Manufacturer>();
        CreateMap<Manufacturer, ManufacturerResponseDto>();

        // Lot

        CreateMap<LotDto, Lot>();
        CreateMap<Lot, LotResponseDto>();

        // Vaccination-VaccinatedSubject Dtos
        CreateMap<VaccinatedSubjectDto, VaccinatedSubject>();
        CreateMap<VaccinationDto, Vaccination>();

        CreateMap<Vaccination, VaccinationDetailsDto>()
            .ForMember(dest => dest.VaccineName, opt => opt.MapFrom(src => src.Lot.Vaccine.Name))
            .ForMember(dest => dest.VaccinationCenterName, opt => opt.MapFrom(src => src.VaccinationCenter.Name))
            .ForMember(dest => dest.LotNumber, opt => opt.MapFrom(src => src.Lot.LotNumber));

        CreateMap<Vaccination, VaccinationSummaryDto>()
            .ForMember(dest => dest.VaccineName, opt => opt.MapFrom(src => src.Lot.Vaccine.Name))
            .ForMember(dest => dest.VaccinationCenterName, opt => opt.MapFrom(src => src.VaccinationCenter.Name));

        CreateMap<Vaccination, VaccinationPdfDto>()
            .ForMember(dest => dest.VaccineName, opt => opt.MapFrom(src => src.Lot.Vaccine.Name));


        CreateMap<VaccinationCenterDto, VaccinationCenter>();
        CreateMap<VaccinationCenter, VaccinationCenterResponseDto>();

        // VaccinatedSubject

        CreateMap<VaccinatedSubject, VaccinatedSubjectDetailsDto>();
        CreateMap<VaccinatedSubject, VaccinatedSubjectSummaryDto>();

        CreateMap<VaccinatedSubject, VaccinatedSubjectPdfDto>();

        CreateMap<VaccinatedSubject, VaccinatedSubjectAdminDto>()
            .ForMember(dest => dest.ProvinceName, opt => opt.MapFrom(src => src.Province.Name))
            .ForMember(dest => dest.MunicipalityName, opt => opt.MapFrom(src => src.Municipality.Name));


        // AdverseEvent
        CreateMap<AdverseEventDto, AdverseEvent>();



        // CreateMap<AdverseEventSymptom, GetSymptomDto>()
        //         .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Symptom.Id))
        //         .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Symptom.Name));

        CreateMap<AdverseEvent, AdverseEventDetailDto>();

        CreateMap<AdverseEvent, AdverseEventDetailMedicalReviewerDto>();

        CreateMap<AdverseEvent, AdverseEventSummaryDto>();

        CreateMap<AdverseEvent, AdverseEventPdfDto>();


        CreateMap<AdverseEvent, AdverseEventAdminDto>()
            .ForMember(dest => dest.Symptom,
            opt => opt.MapFrom(src =>
            src.Symptom.Name));


        //


        // Reporter
        CreateMap<ReporterDto, Reporter>();

        CreateMap<MedicalReviewer, Reporter>()
                        .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.UserName))
                        .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));

        CreateMap<Reporter, ReporterDetailsDto>();
        CreateMap<Reporter, ReporterSummaryDto>();

        CreateMap<Reporter, ReporterPdfDto>();

        CreateMap<Reporter, ReporterAdminDto>();



        // AefiReport
        CreateMap<PublicAefiReportDto, AefiReport>();
        CreateMap<MedicalReportDto, AefiReport>();

        CreateMap<AefiReport, ReportSectionResponsibleDto>()
        .ForMember(
                dest => dest.GlobalSeverityLevel,
                opt => opt.MapFrom(src =>
                    src.AdverseEvents
                        .OrderByDescending(a => a.SeverityLevel)
                        .Select(a => a.SeverityLevel)
                        .FirstOrDefault()
                )
            )
        .ForMember(
            dest => dest.LastDoctorName,
            opt => opt.MapFrom(src =>
                src.MedicalReviewAssignments
                    .OrderByDescending(mra => mra.AssignedAt)
                    .Select(mra => mra.MedicalReviewer.User.UserName)
                    .FirstOrDefault()
            )
        );

        CreateMap<AefiReport, ReportMedicalReviewerDto>()
            .ForMember(
                dest => dest.AssignedDate,
                opt => opt.MapFrom(src =>
                    src.MedicalReviewAssignments
                        .OrderByDescending(mra => mra.AssignedAt)
                        .Select(mra => mra.AssignedAt)
                        .FirstOrDefault()
                    )
            );

        CreateMap<AefiReport, ReportUserDto>();

        CreateMap<AefiReport, ReportPdfDto>();

        CreateMap<AefiReport, ReportSummaryAdminDto>()
            .ForMember(
                dest => dest.GlobalSeverityLevel,
                opt => opt.MapFrom(src =>
                    src.AdverseEvents
                        .OrderByDescending(a => a.SeverityLevel)
                        .Select(a => a.SeverityLevel)
                        .FirstOrDefault()
                )
            )
            .ForMember(dest => dest.VaccinesName, opt => opt.MapFrom(src => src.Vaccinations.Select(v => v.Lot.Vaccine.Name)))
            .ForMember(dest => dest.AdverseEventsName, opt => opt.MapFrom(src => src.AdverseEvents.Select(a => a.Symptom.Name)));


        CreateMap<AefiReport, ReportDetailAdminDto>()
            .ForMember(
                dest => dest.GlobalSeverityLevel,
                opt => opt.MapFrom(src =>
                    src.AdverseEvents
                        .OrderByDescending(a => a.SeverityLevel)
                        .Select(a => a.SeverityLevel)
                        .FirstOrDefault()
                )
            )
    .ForMember(
        dest => dest.MedicalReview,
        opt => opt.MapFrom(src =>
            src.MedicalReviewAssignments
                .Where(a => a.MedicalReview != null)
                .Select(a => a.MedicalReview)
                .OrderByDescending(r => r!.ReviewedAt)
                .FirstOrDefault()
        )
    );

        // medicalAssignment - Medical Reviews Dtos

        CreateMap<MedicalReviewDto, MedicalReview>();
        CreateMap<MedicalReview, MedicalReviewResponseDto>();

        CreateMap<MedicalReviewAssignmentDTO,
                 MedicalReviewAssignment>();


        CreateMap<MedicalReviewAssignment, ReportUserDto>();
        CreateMap<MedicalReviewAssignment, ReportMedicalReviewerDto>();

        CreateMap<MedicalReviewAssignment, AssignmentResponse>()
            .ForMember(dest => dest.MedicalReviewerName, opt => opt.MapFrom(src => src.MedicalReviewer.User.UserName))
            .ForMember(dest => dest.SectionResponsibleName, opt => opt.MapFrom(src => src.SectionResponsible.User.UserName));

    }
}