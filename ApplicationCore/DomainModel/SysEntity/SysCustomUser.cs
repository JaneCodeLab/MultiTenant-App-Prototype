
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.DomainModel;

public class SysCustomUser : IdentityUser
{
    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    public string TimeZone { get; set; } = GeneralVariables.DefaultTimeZone;

    public string? ProfileImage { get; set; }

    public Language Language { get; set; } = Language.English;

    public bool Active { get; set; } = true;

    public Gender Gender { get; set; } = Gender.Male;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? CreatedBy { get; set; } = null!;

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public bool IsEmployeeUser { get; set; } = false;
}