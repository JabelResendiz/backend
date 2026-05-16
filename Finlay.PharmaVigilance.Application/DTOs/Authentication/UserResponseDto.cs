namespace Finlay.PharmaVigilance.Application.DTO.Authentication;

public class UserResponseDTO
{
    public Guid Id { get; set; }
    public required string UserName { get; set; }
    public required string UserRole { get; set; }
    public required string Token { get; set; }
    public required string RefreshToken { get; set; }
}