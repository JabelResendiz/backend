namespace Finlay.PharmaVigilance.Application.DTO;

public class MedicalReviewerFilterDto
{
    public string? Search { get; set; }
    public string? Speciality { get; set; }
    public string? SortBy { get; set; }
    public string? Order { get; set; }
}