
namespace ApplicationService.CustomModels.ApiModels;

public class TokenRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string UserName { get; set; }
}