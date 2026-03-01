
namespace Finlay.PharmaVigilance.Domain.Entities;

public class Employee : GenericEntity
{
    public string Name {get;set;} = null!;
    public string UserRole {get;set;} = null!;
    public string Email {get;set;} = null!;
    public string UserName {get;set;} = null!;
    public User? User {get;set;}
}