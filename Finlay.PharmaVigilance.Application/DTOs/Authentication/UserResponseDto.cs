namespace Finlay.PharmaVigilance.Application.DTO.Authentication;

public class UserResponseDTO
{
    public int Id {get;set;}
    public required string UserName {get; set;}
    public required string Email {get; set;} 
    public required string UserRole {get; set;} 
    public required string Token {get;set;}
}