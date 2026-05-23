namespace Finlay.PharmaVigilance.Domain.Enum;

public enum ReportStatus
{
    Draft = 0,          // El usuario lo está llenando (opcional)
    Submitted = 1,      // Enviado por el reportero
    UnderReview = 2,    // En revisión por un médico/revisor
    Reopened = 3,       // Una asignación expiró y el reporte volvió a estar disponible
    Approved = 4,       // Validado
    Rejected = 5,       // Rechazado (datos incorrectos)
    Closed = 6          // Caso finalizado
}