
using System.Runtime.InteropServices;
using AutoMapper;
using Finlay.PharmaVigilance.Application.DTO.Authentication;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.DTO;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        
        CreateMap<EmployeeDto, Employee>();
        CreateMap<Employee, EmployeeDto>();
        CreateMap<Employee, GetEmployeeDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

        //CreateMap<LoginUserDto, User>();

        CreateMap<RegisterUserDto, User>();

        // CreateMap<RegisterUserDto, Employee>();

        CreateMap<User, UserResponseDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.UserRole));
    }
}

