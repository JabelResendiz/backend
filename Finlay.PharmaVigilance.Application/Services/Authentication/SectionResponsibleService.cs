using AutoMapper;
using Finlay.PharmaVigilance.Application.Authentication;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.DTO.Authentication;
using Finlay.PharmaVigilance.Application.IServices.Authentication;
using Finlay.PharmaVigilance.Application.IServices.Common;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Application.Services.Authentication;

/// <summary>
/// Service implementation for managing Section Responsible registration and authentication.
/// </summary>
public class SectionResponsibleService : ISectionResponsibleService
{
    private readonly IIdentityManager _identityManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    /// <summary>
    /// Initializes a new instance of the SectionResponsibleService class.
    /// </summary>
    public SectionResponsibleService(
        IIdentityManager identityManager,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IUserContextService userContextService)
    {
        _identityManager = identityManager;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    /// <summary>
    /// Registers a new Section Responsible user with their profile information.
    /// </summary>
    public async Task<string> RegisterSectionResponsibleAsync(RegisterSectionResponsibleDto registerDto)
    {
        // Validate inputs
        if (registerDto == null)
            throw new ArgumentNullException(nameof(registerDto), "Registration DTO cannot be null.");

        var userId = _userContextService.GetUserId();

        var administrator = await _unitOfWork.GetRepository<Admin>()
                                        .FirstOrDefaultAsync(sr => sr.UserId == userId);

        if (administrator == null)
            throw new UnauthorizedAccessException("User is not an administrato.");

        // Validate that province exists
        var province = await _unitOfWork.GetRepository<Province>().GetByIdAsync(registerDto.ProvinceId);
        if (province == null)
            throw new KeyNotFoundException($"Province with ID {registerDto.ProvinceId} does not exist.");

        var municipality = await _unitOfWork.GetRepository<Municipality>().GetByIdAsync(registerDto.MunicipalityId);
        if (municipality == null)
            throw new KeyNotFoundException($"Municipality with ID {registerDto.MunicipalityId} does not exist.");

        if (municipality.ProvinceId != province.Id)
            throw new ArgumentException($"The municipality {registerDto.MunicipalityId} does not belong to province {registerDto.ProvinceId}.");

        var user = _mapper.Map<User>(registerDto);

        user.UserRole = UserRole.SectionResponsible.ToString();

        var createdUser = await _identityManager.CreateUserAsync(user, registerDto.Password);
        if (createdUser == null)
            throw new InvalidOperationException("Failed to create user account.");

        // Assign SectionResponsible role
        await _identityManager.AddRoles(createdUser.Id.ToString(), UserRole.SectionResponsible.ToString());

        var sectionResponsible = _mapper.Map<SectionResponsible>(registerDto);
        sectionResponsible.UserId = createdUser.Id;
        sectionResponsible.User = createdUser;
        sectionResponsible.AdminId = administrator.Id;
        sectionResponsible.Admin = administrator;

        // Add to repository and save
        await _unitOfWork.GetRepository<SectionResponsible>().CreateAsync(sectionResponsible);
        await _unitOfWork.CompleteAsync();

        return "Section Responsible successfully registered";
    }

    public async Task<PagedResultDto<SectionResponsibleResponseDto>> SearchByMunicipality(PagedRequestDto paged, int municipalityId)
    {
        Console.WriteLine("898989");

        var query = _unitOfWork.GetRepository<SectionResponsible>()
                                        .GetAllByItems(src => src.MunicipalityId == municipalityId);

        var totalCount = await query.CountAsync();

        var items = await _unitOfWork.GetRepository<SectionResponsible>()
                        .GetAllPagedbyItem((paged.PageNumber - 1) * paged.PageSize,
                                            paged.PageSize,
                                            src => src.MunicipalityId == municipalityId,
                                            sr => sr.User)
                        .ToListAsync();

        return new PagedResultDto<SectionResponsibleResponseDto>
        {
            Items = items?.Select(_mapper.Map<SectionResponsibleResponseDto>) ?? Enumerable.Empty<SectionResponsibleResponseDto>(),
            TotalCount = totalCount,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize,
            NextPageUrl = paged.PageNumber * paged.PageSize < totalCount
                        ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber + 1}&pageSize={paged.PageSize}"
                        : null,
            PreviousPageUrl = paged.PageNumber > 1
                        ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber - 1}&pageSize={paged.PageSize}"
                        : null
        };
    }


    public async Task<PagedResultDto<SectionResponsibleResponseDto>> GetByFilters(
        PagedRequestDto paged,
        string? search,
        string? provinceName,
        int? municipalityId)
    {

        Console.WriteLine($"======================{municipalityId}=====================");


        var query = _unitOfWork.GetRepository<SectionResponsible>()
                                        .GetAllByItems(src => (string.IsNullOrEmpty(search) ||
                                                              src.User.UserName!.Contains(search)) &&
                                                              (string.IsNullOrEmpty(provinceName) ||
                                                              src.Province.Name.Contains(provinceName)) &&
                                                              (!municipalityId.HasValue ||
                                                              src.MunicipalityId == municipalityId.Value));

        var totalCount = await query.CountAsync();

        var items = await _unitOfWork.GetRepository<SectionResponsible>()
                        .GetAllPagedbyItem((paged.PageNumber - 1) * paged.PageSize,
                                            paged.PageSize,
                                            src => (string.IsNullOrEmpty(search) ||
                                                   src.User.UserName!.Contains(search)) &&
                                                   (string.IsNullOrEmpty(provinceName) ||
                                                   src.Province.Name.Contains(provinceName)) &&
                                                   (!municipalityId.HasValue ||
                                                   src.MunicipalityId == municipalityId.Value),
                                            sr => sr.User)
                        .ToListAsync();

        return new PagedResultDto<SectionResponsibleResponseDto>
        {
            Items = items?.Select(_mapper.Map<SectionResponsibleResponseDto>) ?? Enumerable.Empty<SectionResponsibleResponseDto>(),
            TotalCount = totalCount,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize,
            NextPageUrl = paged.PageNumber * paged.PageSize < totalCount
                        ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber + 1}&pageSize={paged.PageSize}"
                        : null,
            PreviousPageUrl = paged.PageNumber > 1
                        ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber - 1}&pageSize={paged.PageSize}"
                        : null
        };
    }
}
