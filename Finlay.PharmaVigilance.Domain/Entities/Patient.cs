using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Domain.Entities;

public class Patient : GenericEntity 
{
    public string FullName {get;set;} = null!;
    public string Address {get;set;} = null!;
    public int Age {get;set;}
    public DateTime DateOfBirth {get;set;}
    public Gender Gender {get;set;}
    public Province Province {get;set;}
}