using Microsoft.AspNetCore.Identity;

namespace Finlay.PharmaVigilance.Domain.Entities;


public class User: IdentityUser<int> {

    public string UserRole {get;set;} = null!;
    public DateTime CreatedAt {get;set;}
    public DateTime UpdatedAt {get;set;}
    //public string Token {get;set;} = null!;
    //public string RefreshToken {get;set;} = null!;
    
}

public class Role : IdentityRole<int>{
    
}