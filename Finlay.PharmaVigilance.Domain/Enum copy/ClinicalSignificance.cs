namespace Finlay.PharmaVigilance.Domain.Enum;

public enum ClinicalSignificance
{
    // Clínicamente significativo e inesperado
    ClinicallySignificantAndUnexpected,

    // Evento esperado
    ExpectedEvent,

    // Evento serio o potencialmente mortal
    SeriousOrLifeThreatening,

    // Evento menor
    MinorEvent
}