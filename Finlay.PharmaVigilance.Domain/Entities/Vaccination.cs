namespace Finlay.PharmaVigilance.Domain.Entities;


public class Vaccination : GenericEntity
{
    public string BatchNumber {get;set;} = null!;
    public string AdministrationSite {get;set;} = null!;
    public int DoseNumber {get;set;}
    public DateTime CreatedAt {get;set;}
    public DateTime AdministrationDate {get;set;}
}