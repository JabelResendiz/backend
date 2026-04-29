using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Domain.Enum;


public enum ReporterRelationship
{
    Self,
    Parent,
    Guardian,
    Relative,
    Doctor,
    Nurse,
    Other
}



// public static class ReporterRelationshipHelper
// {
//     /// <summary>
//     /// Validates whether the specified role belongs to any of the registered roles
//     /// </summary>
//     public static bool IsValidRelationship(string relationship) =>
//         System.Enum.TryParse(typeof(ReporterRelationship), relationship, out _);

// }