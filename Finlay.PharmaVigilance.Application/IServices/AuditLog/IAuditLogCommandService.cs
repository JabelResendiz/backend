using Finlay.PharmaVigilance.Application.DTO;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IAuditLogCommandService : IGenericCommandService<AuditLogDto, AuditLogDto>
{

}