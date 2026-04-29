using System.ComponentModel.DataAnnotations;

namespace Finlay.PharmaVigilance.Application.DTO;

public class CreateContactDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "The email is not in a valid format.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Name is required")]
    [MinLength(1, ErrorMessage = "Name cannot be empty or whitespace.")]
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Department { get; set; }
}

public class ContactDto
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Department { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SendEmailDto
{
    [Required(ErrorMessage = "ContactID is required")]
    public Guid ContactId { get; set; }

    [Required(ErrorMessage = "Subject is required")]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Message is required")]
    [MinLength(1, ErrorMessage = "Message cannot be empty or whitespace.")]
    public string Message { get; set; } = string.Empty;
}
