namespace Finlay.PharmaVigilance.Application.DTO;

public class AuditLogDto
{
    public Guid? UserId { get; set; }
    public Guid? ReporterId { get; set; }
    public string Action { get; set; } = null!;
    public string EntityName { get; set; } = null!;
    public Guid? EntityId { get; set; }
    public string? Details { get; set; }
    public string? NewValues { get; set; }
    public string? IpAddress { get; set; }
}