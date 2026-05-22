using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.DTO.Authentication;

namespace Finlay.PharmaVigilance.Application.IServices.Authentication;

/// <summary>
/// Service interface for managing Medical Reviewer registration and authentication.
/// </summary>
public interface IMedicalReviewerService
{
    /// <summary>
    /// Registers a new Medical Reviewer user with their profile information.
    /// Creates both a User account and a MedicalReviewer profile.
    /// </summary>
    /// <param name="registerDto">The DTO containing registration and profile details.</param>
    /// <returns>A task representing the asynchronous operation, returning a response message.</returns>
    Task<string> RegisterMedicalReviewerAsync(RegisterMedicalReviewerDto registerDto);

    Task<IEnumerable<GetMedicalReviewerDto>> ListByMunicipalityAsync(int municipalityId, CancellationToken cancellationToken = default);

    // Task<IEnumerable<GetMedicalReviewerDto>> ListByProvinceAsync(CancellationToken cancellationToken = default);

    Task<PagedResultDto<GetMedicalReviewerDto>> GetMedicalReviewerForCurrentUserAsync(
        PagedRequestDto paged,
        MedicalReviewerFilterDto? filter);

}
