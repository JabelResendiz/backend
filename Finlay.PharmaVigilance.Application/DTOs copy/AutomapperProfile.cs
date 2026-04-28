
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
        //CreateMap<Vaccine, GetPrivateVaccineDto>();

        CreateMap<SymptomDto, Symptom>();
        CreateMap<Symptom, GetSymptomDto>();
        // CreateMap<Symptom, GetPrivateSymptomsDto>();

        // Vaccination-VaccinatedSubject Dtos
        CreateMap<VaccinatedSubjectDto, VaccinatedSubject>();
        CreateMap<VaccinationDto, Vaccination>();





        // AdverseEvent - AefiReport - Reporter
        CreateMap<AdverseEventDto, AdverseEvent>()
            .ForMember(dest => dest.AdverseEventSymptoms,
                opt => opt.MapFrom(src =>
                    src.Symptoms.Select(id => new AdverseEventSymptom
                    {
                        SymptomId = id
                    })
                )
            );

        CreateMap<ReporterDto, Reporter>();


        CreateMap<PublicAefiReportDto, AefiReport>();
        CreateMap<MedicalReportDto, AefiReport>();

        // Email
        // CreateMap<CreateContactDto, Contact>()
        //     .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Trim().ToLowerInvariant()))
        //     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()))
        //     .ForMember(dest => dest.Phone, opt => opt.MapFrom(src =>
        //             src.Phone != null ? Regex.Replace(src.Phone.Trim(), "[^0-9+]", "") : null))
        //     .ForMember(dest => dest.Department, opt => opt.MapFrom(src =>
        //             src.Department != null ? src.Department.Trim() : null));
        CreateMap<MedicalReviewer, Reporter>()
                        .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.UserName))
                        .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));



        // medicalAssignment - Medical Reviews Dtos

        CreateMap<MedicalReviewDto, MedicalReview>();
        CreateMap<MedicalReviewAssignmentDTO,
                 MedicalReviewAssignment>();

        // Contact Dtos
        // CreateMap<CreateContactDto, Contact>()
        //     .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Trim().ToLowerInvariant()))
        //     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()))
        //     .ForMember(dest => dest.Phone, opt => opt.MapFrom(src =>
        //             src.Phone != null ? Regex.Replace(src.Phone.Trim(), "[^0-9+]", "") : null))
        //     .ForMember(dest => dest.Department, opt => opt.MapFrom(src =>
        //             src.Department != null ? src.Department.Trim() : null));


        // CreateMap<Contact, ContactDto>();

        // ResponseDto

        CreateMap<VaccinatedSubject, VaccinatedSubjectResponseDto>();
        CreateMap<Reporter, ReporterResponseDto>();
        CreateMap<Vaccination, VaccinationResponseDto>()
            .ForMember(dest => dest.VaccineName, opt => opt.MapFrom(src => src.Vaccine.Name));

        CreateMap<AdverseEvent, AdverseEventDetailDto>();

        // CreateMap<AefiReport, ReportResponseDto>()
        //     .ForMember(dest => dest.VaccinatedSubject, opt => opt.MapFrom(src => src.VaccinatedSubject))
        //     .ForMember(dest => dest.Reporter, opt => opt.MapFrom(src => src.Reporter));

        // ReportResponseSimpleDto
        CreateMap<AefiReport, ReportSummaryDto>();
        CreateMap<AdverseEvent, AdverseEventSummaryDto>();
        CreateMap<Vaccination, VaccinationSummaryDto>()
                .ForMember(dest => dest.VaccineName, opt => opt.MapFrom(src => src.Vaccine.Name));
        CreateMap<AdverseEvent, AdverseEventSummaryDto>();

    }
}