namespace Finlay.PharmaVigilance.Application.DTO;


public class CompleteRegistrationDto
{
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ProfessionalNumber { get; set; } = null!;
}