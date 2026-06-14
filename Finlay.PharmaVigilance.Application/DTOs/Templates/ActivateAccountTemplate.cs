using System.Text.Json.Serialization;

namespace Finlay.PharmaVigilance.Application.DTO;


public class ActivateAccountTemplate : IBasicTemplate
{
    [JsonPropertyName("reviewer_name")]
    public string FullName { get; set; } = null!;

    [JsonPropertyName("activation_url")]
    public string ActivationUrl { get; set; } = null!;
}