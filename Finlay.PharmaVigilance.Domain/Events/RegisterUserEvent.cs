namespace Finlay.PharmaVigilance.Domain.Events;


public class RegisterUserEvent : BasicEvent
{
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Token { get; set; } = null!;
}