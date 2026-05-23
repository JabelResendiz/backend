using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IRepository;

/// <summary>
/// Repository interface for managing MedicalReviewer entities.
/// </summary>
public interface IMedicalAssignmentRepository : IGenericRepository<MedicalReviewAssignment>
{

    Task<ICollection<DoctorPerformanceDto>> GetDoctorPerformanceAsync(int municipalityId);

    Task<ICollection<TimeHourDto>> GetTimeHoursAsync(int municipalityId);

    Task<MunicipalMetricsDto> GetMetrics(Guid sectionResponsibleId);
}
