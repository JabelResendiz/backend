namespace Finlay.PharmaVigilance.Domain.Entities;


public class AefiReport : GenericEntity
{
    public DateTime ReportDate {get;set;}
    public string GeneralNotes {get;set;} = null!;
    public DateTime CreatedAt {get;set;}
    
}