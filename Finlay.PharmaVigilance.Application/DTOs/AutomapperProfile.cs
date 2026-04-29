
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
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber));

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

        CreateMap<SymptomDto, Symptom>();
        CreateMap<Symptom, GetSymptomDto>();
        CreateMap<Symptom, GetPrivateSymptomsDto>();




        // Vaccination-VaccinatedSubject Dtos
        CreateMap<VaccinatedSubjectDto, VaccinatedSubject>();
        CreateMap<VaccinationDto, Vaccination>();

        CreateMap<Vaccination, VaccinationDetailsDto>()
            .ForMember(dest => dest.VaccineName, opt => opt.MapFrom(src => src.Vaccine.Name));

        CreateMap<Vaccination, VaccinationSummaryDto>()
            .ForMember(dest => dest.VaccineName, opt => opt.MapFrom(src => src.Vaccine.Name));

        CreateMap<VaccinatedSubject, VaccinatedSubjectDetailsDto>();
        CreateMap<VaccinatedSubject, VaccinatedSubjectSummaryDto>();



        // AdverseEvent
        CreateMap<AdverseEventDto, AdverseEvent>()
            .ForMember(dest => dest.AdverseEventSymptoms,
                opt => opt.MapFrom(src =>
                    src.Symptoms.Select(id => new AdverseEventSymptom
                    {
                        SymptomId = id
                    })
                )
            );



        CreateMap<AdverseEventSymptom, GetSymptomDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Symptom.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Symptom.Name));

        CreateMap<AdverseEvent, AdverseEventDetailDto>()
                .ForMember(dest => dest.Symptoms,
            opt => opt.MapFrom(src =>
                src.AdverseEventSymptoms.Select(x => x.Symptom)
            )
        );

        CreateMap<AdverseEvent, AdverseEventSummaryDto>();




        // Reporter
        CreateMap<ReporterDto, Reporter>();

        CreateMap<MedicalReviewer, Reporter>()
                        .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.UserName))
                        .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));

        CreateMap<Reporter, ReporterDetailsDto>();
        CreateMap<Reporter, ReporterSummaryDto>();




        // AefiReport
        CreateMap<PublicAefiReportDto, AefiReport>();
        CreateMap<MedicalReportDto, AefiReport>();

        CreateMap<AefiReport, ReportSectionResponsibleDto>();
        CreateMap<AefiReport, ReportMedicalReviewerDto>();
        CreateMap<AefiReport, ReportUserDto>();


        // medicalAssignment - Medical Reviews Dtos

        CreateMap<MedicalReviewDto, MedicalReview>();
        CreateMap<MedicalReviewAssignmentDTO,
                 MedicalReviewAssignment>();


        CreateMap<MedicalReviewAssignment, ReportUserDto>();
        CreateMap<MedicalReviewAssignment, ReportMedicalReviewerDto>();

    }
}