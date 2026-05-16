
using System.Text;
using Finlay.PharmaVigilance.Domain.Enum;
using System.Globalization;

namespace Finlay.PharmaVigilance.Domain.Entities;

public class Vaccine : GuidEntity
{
    private string _name = null!;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            NormalizedName = NormalizeName(value);
        }
    }

    public string? NormalizedName { get; private set; }

    public VaccineType Type { get; set; }
    public string? Description { get; set; }
    public string? Code { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public bool IsActive { get; set; } = true;
    // public string Manufacturer { get; set; } = null!;
    public Guid ManufacturerId { get; set; }
    public Manufacturer Manufacturer { get; set; } = null!;

    public ICollection<Vaccination> Vaccinations { get; set; } = new List<Vaccination>();

    /// <summary>
    /// Normalizes a vaccine name: converts to uppercase and removes accents
    /// </summary>
    public static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return name;

        // Convert to uppercase
        string upperName = name.ToUpper(CultureInfo.InvariantCulture);

        // Remove accents
        string normalizedName = RemoveAccents(upperName);

        return normalizedName;
    }

    /// <summary>
    /// Removes accents and diacritical marks from a string
    /// </summary>
    private static string RemoveAccents(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        string formD = text.Normalize(NormalizationForm.FormD);
        StringBuilder sb = new StringBuilder();

        foreach (char c in formD)
        {
            UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);
            if (uc != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}