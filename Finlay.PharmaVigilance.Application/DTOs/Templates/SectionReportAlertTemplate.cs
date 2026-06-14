
using System.Text.Json.Serialization;

namespace Finlay.PharmaVigilance.Application.DTO;


public class SectionReportAlertTemplate : IBasicTemplate
{
    [JsonPropertyName("receipt_number")]
    public string NotificationNumber { get; set; } = default!;

}