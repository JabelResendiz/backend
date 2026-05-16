using System.ComponentModel.DataAnnotations;

namespace Finlay.PharmaVigilance.Application.DTO;


public class LotDto
{
    [Required(ErrorMessage = "Lot Number is required.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Lot Number must be between 1 and 50 characters.")]
    public string LotNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vaccine is required.")]
    public Guid VaccineId { get; set; }
}

public class LotResponseDto
{
    public required Guid Id { get; set; }
    public required string LotNumber { get; set; }
}