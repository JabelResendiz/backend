namespace Finlay.PharmaVigilance.Domain.Enum;


public enum Province
{
   PinardelRio,
   Artemisa,
   Mayabeque,
   IslaJuventud,
   LaHabana,
   Matanzas,
   Cienfuegos,
   VillaClara,
   SanctiSpiritus,
   CiegoAvila,
   Camaguey,
   LasTunas,
   Granma,
   Holguín,
   SantiagoCuba,
   Guantanamo
}


public static class ProvinceHelper
{
    /// <summary>
    /// Validates whether the specified province belongs to any of the registered provinces
    /// </summary>
    public static bool IsValidProvince(string province) =>
        System.Enum.TryParse(typeof(Province), province, out _);

    /// <summary>
    /// Retrieves all provinces defined in the Provinces enum as a collection of strings.
    /// </summary>
    /// <returns>A read-only collection containing all provinces names.</returns>
    public static IReadOnlyCollection<string> AllProvinces() =>
        System.Enum.GetNames(typeof(Province));

}
