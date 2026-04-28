using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IRepository;

/// <summary>
/// Repository interface for managing MedicalReviewer entities.
/// </summary>
public interface IMedicalReviewerRepository : IGenericRepository<MedicalReviewer>
{
    /// <summary>
    /// Retrieves a MedicalReviewer by their associated User ID.
    /// </summary>
    /// <param name="userId">The User ID to search for.</param>
    /// <returns>The MedicalReviewer entity if found; otherwise, null.</returns>
    Task<MedicalReviewer?> GetByUserIdAsync(int userId);

    /// <summary>
    /// Retrieves all MedicalReviewers for a specific province.
    /// </summary>
    /// <param name="provinceId">The Province ID to filter by.</param>
    /// <returns>A collection of MedicalReviewer entities for the specified province.</returns>
    Task<IEnumerable<MedicalReviewer>> GetByProvinceAsync(int provinceId);

    /// <summary>
    /// Retrieves all MedicalReviewers for a specific municipality.
    /// </summary>
    /// <param name="municipalityId">The Municipality ID to filter by.</param>
    /// <returns>A collection of MedicalReviewer entities for the specified municipality.</returns>
    Task<IEnumerable<MedicalReviewer>> GetByMunicipalityAsync(int municipalityId);

    /// <summary>
    /// Checks if a MedicalReviewer already exists for a given User ID.
    /// </summary>
    /// <param name="userId">The User ID to check.</param>
    /// <returns>True if a MedicalReviewer exists for this User ID; otherwise, false.</returns>
    Task<bool> ExistsByUserIdAsync(int userId);
}
