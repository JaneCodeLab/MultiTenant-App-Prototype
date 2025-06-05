
namespace Infrastructure.Captcha;

public class RecaptchaV3Response
{
    public bool success { get; set; }
    public double score { get; set; }
    public string action { get; set; }
    public string challenge_ts { get; set; }
    public string hostname { get; set; }
}