using Microsoft.AspNetCore.Identity;

namespace Finlay.PharmaVigilance.Domain.Entities;


public class User : IdentityUser<Guid>
{

    public string UserRole { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}

public class Role : IdentityRole<Guid>
{

}