
using Infrastructure.Captcha;
using Microsoft.Extensions.Options;

namespace ApplicationService;

public class CaptchaService : ICaptchaService
{
    private readonly IOptionsMonitor<CaptchaConfig> _config;
    public CaptchaService(IOptionsMonitor<CaptchaConfig> config)
    {
        _config = config;
    }

    public async Task<bool> VerifyRecaptchaV3Token(string token)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(_config.CurrentValue.RecaptchaV3SecretKey))
            return false;
        return await RecaptchaV3Adapter.VerifyToken(_config.CurrentValue.RecaptchaV3SecretKey, token, 0.5);
    }
}