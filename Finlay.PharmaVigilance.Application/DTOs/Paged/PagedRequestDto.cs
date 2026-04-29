using System.ComponentModel.DataAnnotations;

namespace Finlay.PharmaVigilance.Application.DTO;


public class PagedRequestDto
{
    [Required(ErrorMessage = "PageNumber is required")]
    public int PageNumber { get; set; }
    [Required(ErrorMessage = "PageSize is required")]
    [Range(1, 500, ErrorMessage = "Page Size cannot exceed 500")]
    public int PageSize { get; set; }
    public string? BaseUrl { get; set; }
}