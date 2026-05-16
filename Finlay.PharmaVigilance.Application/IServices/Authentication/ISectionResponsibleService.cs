using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.DTO.Authentication;

namespace Finlay.PharmaVigilance.Application.IServices.Authentication;

/// <summary>
/// Service interface for managing Section Responsible registration and authentication.
/// </summary>
public interface ISectionResponsibleService
{
    /// <summary>
    /// Registers a new Section Responsible user with their profile information.
    /// Creates both a User account and a SectionResponsible profile.
    /// </summary>
    /// <param name="registerDto">The DTO containing registration and profile details.</param>
    /// <returns>A task representing the asynchronous operation, returning a response message.</returns>
    Task<string> RegisterSectionResponsibleAsync(RegisterSectionResponsibleDto registerDto);


    Task<PagedResultDto<SectionResponsibleResponseDto>> SearchByMunicipality(PagedRequestDto paged, int municipalityId);

    Task<PagedResultDto<SectionResponsibleResponseDto>> GetByFilters(
        PagedRequestDto paged,
        string? search,
        string? provinceName,
        int? municipalityId);
}
