using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Infrastructure;
using Finlay.PharmaVigilance.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Application.Repository;

/// <summary>
/// Repository implementation for managing MedicalReviewer entities.
/// </summary>
public class MedicalReviewerRepository : GenericRepository<MedicalReviewer>, IMedicalReviewerRepository
{
    /// <summary>
    /// Initializes a new instance of the MedicalReviewerRepository class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public MedicalReviewerRepository(FinlayDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Retrieves a MedicalReviewer by their associated User ID.
    /// </summary>
    public async Task<MedicalReviewer?> GetByUserIdAsync(int userId)
    {
        return await _entity
            .Include(mr => mr.User)
            .Include(mr => mr.Province)
            .Include(mr => mr.Municipality)
            .FirstOrDefaultAsync(mr => mr.UserId == userId);
    }

    /// <summary>
    /// Retrieves all MedicalReviewers for a specific province.
    /// </summary>
    public async Task<IEnumerable<MedicalReviewer>> GetByProvinceAsync(int provinceId)
    {
        return await _entity
            .Where(mr => mr.ProvinceId == provinceId)
            .Include(mr => mr.User)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves all MedicalReviewers for a specific municipality.
    /// </summary>
    public async Task<IEnumerable<MedicalReviewer>> GetByMunicipalityAsync(int municipalityId)
    {
        return await _entity
            .Where(mr => mr.MunicipalityId == municipalityId)
            .Include(mr => mr.User)
            .ToListAsync();
    }

    /// <summary>
    /// Checks if a MedicalReviewer already exists for a given User ID.
    /// </summary>
    public async Task<bool> ExistsByUserIdAsync(int userId)
    {
        return await _entity
            .AnyAsync(mr => mr.UserId == userId);
    }
}
