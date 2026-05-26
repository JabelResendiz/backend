namespace Finlay.PharmaVigilance.Infrastructure.Settings;

public class EmailJsSettings
{
    public string ServiceId { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public string AccessToken { get; set; } = default!;
    public string ActivateAccount { get; set; } = default!;
    public string SelfReportConfirmation { get; set; } = default!;
    public string AssignmentExpired { get; set; } = default!;
    public string SectionReportAlert { get; set; } = default!;
    public string MedicalReviewerAssignment { get; set; } = default!;

}