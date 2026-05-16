using System.Text;
using System.Globalization;

namespace Finlay.PharmaVigilance.Domain.Entities;

public class Symptom : GuidEntity
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

    public string? Description { get; set; }
    public string StandardCode { get; set; } = null!;
    public string CodingSystem { get; set; } = null!;
    public string Category { get; set; } = null!;
    public bool IsActive { get; set; }

    public ICollection<AdverseEvent> AdverseEvents { get; set; } = new List<AdverseEvent>();

    /// <summary>
    /// Normalizes a symptom name: converts to uppercase and removes accents
    /// </summary>
    private static string NormalizeName(string name)
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