
using System.Linq;
using System.Security.Claims;

namespace Presentation
{
    public static class CustomExtentions
    {
        public static string GetClaim(this ClaimsIdentity claimsIdentity, string claimType)
        {
            var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == claimType);
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}
