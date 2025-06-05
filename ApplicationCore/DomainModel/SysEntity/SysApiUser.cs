
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.DomainModel;

public class SysApiUser : BaseEntity<int>
{
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { set; get; }
    [Required]
    public string UserName { set; get; }
}