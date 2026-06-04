namespace Finlay.PharmaVigilance.Domain.Entities;

public class AuditLog : GuidEntity
{
    // Usuario autenticado (nulo si es anónimo)
    public Guid? UserId { get; set; }
    public User? User { get; set; }

    // Reportero anónimo (nulo si es usuario autenticado)
    public Guid? ReporterId { get; set; }
    public Reporter? Reporter { get; set; }

    // Datos de la operación
    public string Action { get; set; } = null!;
    public string EntityName { get; set; } = null!;
    public Guid? EntityId { get; set; }
    public string? Details { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? IpAddress { get; set; }
}