
namespace ApplicationService;

public interface ICaptchaService
{
    Task<bool> VerifyRecaptchaV3Token(string token);
}