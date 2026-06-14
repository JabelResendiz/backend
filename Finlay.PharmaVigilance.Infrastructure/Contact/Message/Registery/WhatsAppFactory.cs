using Finlay.PharmaVigilance.Application.DTO;

namespace Finlay.PharmaVigilance.Infrastructure.Email;

public static class WhatsAppMessageFactory
{
    public static string Build(IBasicTemplate basicTemplate)
    {
        return basicTemplate switch
        {
            ReportConfirmationTemplate e => BuildReportConfirmation(e),
            AssignmentExpiredTemplate e => BuildAssignmentExpired(e),
            SectionReportAlertTemplate e => BuildSectionAlert(e),
            NewAssignmentTemplate e => BuildMedicalReviewerAssignment(e),
            _ => throw new NotSupportedException(
                $"Event type '{basicTemplate.GetType().Name}' is not supported for WhatsApp messages")
        };
    }

    private static string BuildReportConfirmation(ReportConfirmationTemplate e)
    {
        // return $"✅ *Reporte Creado Exitosamente*\n\n" +
        //        $"Número de notificación: {e.ReportNumber}\n" +
        //        $"Fecha: {e.OccurredOn:dd/MM/yyyy HH:mm}\n\n" +
        //        $"Gracias por contribuir a la farmacovigilancia.";

        return $"✅ *Reporte Creado Exitosamente*\n\n";
    }

    private static string BuildAssignmentExpired(AssignmentExpiredTemplate e)
    {
        // return $"⚠️ *Asignación Vencida*\n\n" +
        //        $"ID Asignación: {e.AssignmentId}\n" +
        //        $"Revisor: {e.ReviewerName}\n" +
        //        $"Fecha de vencimiento: {e.ExpirationDate:dd/MM/yyyy}\n\n" +
        //        $"Por favor revise el sistema.";

        return $"⚠️ *Asignación Vencida*\n\n";
    }

    private static string BuildSectionAlert(SectionReportAlertTemplate e)
    {
        // return $"📊 *Alerta de Sección*\n\n" +
        //        $"Sección: {e.SectionName}\n" +
        //        $"Reportes pendientes: {e.PendingReports}\n" +
        //        $"Prioridad: {e.Priority}\n\n" +
        //        $"Se requiere atención inmediata.";

        return $"📊 *Alerta de Reporte*\n\n";
    }

    private static string BuildMedicalReviewerAssignment(NewAssignmentTemplate e)
    {
        // return $"👨‍⚕️ *Nueva Asignación Médica*\n\n" +
        //        $"Reporte: {e.ReportId}\n" +
        //        $"Revisor médico: {e.ReviewerName}\n" +
        //        $"Plazo: {e.Deadline:dd/MM/yyyy}\n\n" +
        //        $"Ingrese al sistema para revisar.";

        return $"👨‍⚕️ *Nueva Asignación Médica*\n\n";
    }
}