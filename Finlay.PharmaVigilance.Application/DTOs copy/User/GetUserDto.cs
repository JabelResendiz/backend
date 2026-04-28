

namespace Finlay.PharmaVigilance.Application.DTO;


public class GetUserDto
{
    public required string UserName { get; set; }
    public required string UserRole { get; set; }
    public required string Email { get; set; }
    
}