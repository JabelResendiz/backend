using Finlay.PharmaVigilance.Application.DTO;
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
    public async Task<MedicalReviewer?> GetByUserIdAsync(Guid userId)
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
    public async Task<bool> ExistsByUserIdAsync(Guid userId)
    {
        return await _entity
            .AnyAsync(mr => mr.UserId == userId);
    }


    public IQueryable<MedicalReviewer> GetByFilter(
       int provinceId, int municipalityId,
       MedicalReviewerFilterDto? filter)
    {

        var query = _entity.Where(mr => mr.ProvinceId == provinceId && mr.MunicipalityId == municipalityId);

        if (filter == null) return query;

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(mr =>
            mr.User.UserName!.Contains(filter.Search));
        }


        if (!string.IsNullOrWhiteSpace(filter.Speciality))
        {
            query = query.Where(mr =>
           mr.Specialty == filter.Speciality);

        }

        return query;
    }



    public IEnumerable<GetMedicalReviewerDetailDto> OrderAndSort(
        IEnumerable<GetMedicalReviewerDetailDto> query,
        MedicalReviewerFilterDto? filter
    )
    {

        if (filter == null) return query;

        bool asc = filter.Order?.ToLower() == "asc";

        Console.WriteLine(filter.SortBy?.ToLower());

        query = filter.SortBy?.ToLower() switch
        {
            "createdat" => asc
                ? query.OrderBy(r => r.CreatedAt)
                : query.OrderByDescending(r => r.CreatedAt),

            "fullname" => asc
                ? query.OrderBy(r => r.FullName)
                : query.OrderByDescending(r => r.FullName),

            "totalassignments" => asc
                ? query.OrderBy(r => r.TotalAssignments)
                : query.OrderByDescending(r => r.TotalAssignments),

            "averagetimereview" => asc
                ? query.OrderBy(r => r.AverageTimeReview)
                : query.OrderByDescending(r => r.AverageTimeReview),

            "expiredassignments" => asc
                ? query.OrderBy(r => r.ExpiredAssignments)
                : query.OrderByDescending(r => r.ExpiredAssignments),

            "completedassignments" => asc
                ? query.OrderBy(r => r.CompletedAssignments)
                : query.OrderByDescending(r => r.CompletedAssignments),


            _ => query.OrderByDescending(r => r.CreatedAt) // default
        };


        return query;
    }




}
