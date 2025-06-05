
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.DomainModel;

public class SysSmtp : BaseEntity<int>
{
    [Required]
    public string Host { get; set; } = null!;
    [Required]
    public int Port { set; get; }
    [Required]
    public bool EnableSSL { set; get; } = true;
    [Required]
    public bool UseCredential { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    [Required]
    public string EmailAddress { get; set; } = null!;
    [Required]
    public string DisplayName { get; set; } = null!;

}