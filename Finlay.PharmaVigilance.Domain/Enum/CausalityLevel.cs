namespace Finlay.PharmaVigilance.Domain.Enum;

public enum CausalityLevel
{
    // Definitiva: Relación temporal perfecta y sin otras causas
    Definitive,

    // Probable: Relación temporal razonable; poco probable otras causas
    Probable,

    // Posible: Relación razonable, pero otras causas podrían explicarlo
    Possible,

    // Improbable / No relacionada: Existe otra explicación más clara
    Improbable,

    // No evaluable: Información insuficiente para juzgar
    NotEvaluable
}