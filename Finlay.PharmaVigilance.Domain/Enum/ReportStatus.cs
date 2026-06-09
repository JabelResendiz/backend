namespace Finlay.PharmaVigilance.Domain.Enum;

public enum ReportStatus
{
    Draft = 0,          // El reporte está en duda, aún no ha sido verificado
    Submitted = 1,      // Enviado por el reportero
    UnderReview = 2,    // En revisión por un médico/revisor
    Reopened = 3,
    Approved = 4,       // Validado
    Rejected = 5,       // Rechazado (datos incorrectos)
    Closed = 6          // Caso finalizado
}