namespace Finlay.PharmaVigilance.Domain.Enum;

public enum ReportStatus
{
    Draft = 0,          // El usuario lo está llenando (opcional)
    Submitted = 1,      // Enviado por el reportero
    UnderReview = 2,    // En revisión por un médico/revisor
    Approved = 3,       // Validado
    Rejected = 4,       // Rechazado (datos incorrectos)
    Closed = 5          // Caso finalizado
}