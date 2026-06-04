using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.IServices;


public interface IReportDuplicateService
{
    Task<AefiReport?> ValidateDuplicate(AefiReport report);
    Task CreateAsync(ReportDuplicate report);

}