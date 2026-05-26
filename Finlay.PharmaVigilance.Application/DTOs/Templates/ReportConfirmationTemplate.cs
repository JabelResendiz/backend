
using System.Text.Json.Serialization;

namespace Finlay.PharmaVigilance.Application.DTO;


public class ReportConfirmationTemplate
{
    [JsonPropertyName("vaccines")]
    public string Vaccines { get; set; } = default!;
    [JsonPropertyName("symptoms")]
    public string Symptoms { get; set; } = default!;
    [JsonPropertyName("report_date")]
    public string ReportDate { get; set; } = default!;

    [JsonPropertyName("receipt_number")]
    public string NotificationNumber { get; set; } = default!;

    [JsonPropertyName("link")]
    public string Url { get; set; } = null!;
}