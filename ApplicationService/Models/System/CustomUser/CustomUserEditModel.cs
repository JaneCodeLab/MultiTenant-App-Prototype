
using ApplicationCore.DomainModel;
using System.ComponentModel.DataAnnotations;

namespace ApplicationService;

public class CustomUserEditModel
{
    [Required]
    public string Id { get; set; } = null!;

    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = null!;
    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public Language Language { get; set; }
    public bool Active { get; set; }

}