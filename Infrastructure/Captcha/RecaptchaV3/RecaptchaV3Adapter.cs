
using Newtonsoft.Json;
using System.Net;

namespace Infrastructure.Captcha;

public static class RecaptchaV3Adapter
{
    public static async Task<bool> VerifyToken(string secretKey, string token, double acceptableScore)
    {
        var recaptchaUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
        var requestUrl = string.Format(recaptchaUrl, secretKey, token);

        using (var client = new HttpClient())
        {
            var result = await client.GetAsync(requestUrl);
            if (result.StatusCode != HttpStatusCode.OK)
                return false;

            var googleResponse = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<RecaptchaV3Response>(googleResponse);

            return response.success && response.score >= acceptableScore;
        }
    }
}