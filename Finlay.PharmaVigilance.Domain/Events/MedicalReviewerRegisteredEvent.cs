namespace Finlay.PharmaVigilance.Domain.Events;


public class MedicalReviewerRegisteredEvent
{
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Token { get; set; } = null!;
}