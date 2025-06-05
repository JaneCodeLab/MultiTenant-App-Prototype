
using ApplicationCore.DomainModel;
using ApplicationService;
using DocumentFormat.OpenXml.Spreadsheet;
using Infrastructure.Helpers;
using System.Security.Claims;

namespace Presentation
{
    public static class ClaimsPrincipalExtensions
    {

        public static string GetLoggedInUserId(this ClaimsPrincipal principal)
        {
            try
            {
                return principal.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            catch
            {
                return String.Empty;
            }
        }
        public static string GetLoggedInUserName(this ClaimsPrincipal principal)
        {
            try
            {
                return principal.FindFirstValue(ClaimTypes.Name);
            }
            catch
            {
                return String.Empty;
            }
        }
        public static string GetLoggedInFullName(this ClaimsPrincipal principal)
        {
            try
            {
                return principal.FindFirstValue(CustomClaimType.FullName);
            }
            catch
            {
                return String.Empty;
            }
        }
        public static SysCustomUser GetOnlineUser(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return new SysCustomUser
            {
                FirstName = principal.FindFirstValue(CustomClaimType.FullName),
                Language = (Language)Enum.Parse(typeof(Language), principal.FindFirstValue(CustomClaimType.LanguageName), true),
                Id = principal.FindFirstValue(ClaimTypes.NameIdentifier),
            };

        }
        public static string GetLoggedInUserImage(this ClaimsPrincipal principal)
        {
            try
            {
                return principal.FindFirstValue(CustomClaimType.ProfileImage);
            }
            catch
            {
                return String.Empty;
            }
        }
        public static Language GetLoggedInUserLanguageEnum(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            var languageName = principal.FindFirstValue(CustomClaimType.LanguageName);
            if (languageName.IsNullOrEmpty())
                return Language.English;
            else
                return (Language)Enum.Parse(typeof(Language), languageName, true);
        }
        public static int GetLoggedInUserLanguageCode(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            var languageName = principal.FindFirstValue(CustomClaimType.LanguageName);
            if (languageName.IsNullOrEmpty())
                return (int)Language.English;
            else
                return (int)(Language)Enum.Parse(typeof(Language), languageName, true);
        }
        public static TimeZoneInfo GetLoggedInUserTimezone(this ClaimsPrincipal principal)
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(principal.FindFirstValue(CustomClaimType.TimeZone));
            }
            catch
            {
                return TimeZoneInfo.FindSystemTimeZoneById(GeneralVariables.DefaultTimeZone);
            }
        }
        public static string GetLoggedInUserTimezoneId(this ClaimsPrincipal principal)
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(principal.FindFirstValue(CustomClaimType.TimeZone)).Id;
            }
            catch
            {
                return GeneralVariables.DefaultTimeZone;
            }
        }
        public static string? GetCurrentTenantName(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            try
            {
                return TenantHelper.Get(principal.FindFirstValue(CustomClaimType.TenantId).ToInt())?.Title;
            }
            catch
            {
                return "Unknown";
            }
        }
        public static int GetCurrentTenantId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            try
            {
                return principal.FindFirstValue(CustomClaimType.TenantId).ToInt();
            }
            catch
            {
                return 0;
            }
        }
        public static string GetCurrentTenantLogo(this ClaimsPrincipal principal)
        {
            try
            {
                return TenantHelper.Get(principal.FindFirstValue(CustomClaimType.TenantId).ToInt())?.Logo;
            }
            catch
            {
                return "logo.png";
            }
        }
        public static string GetLoggedInUserSquad(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirstValue(CustomClaimType.Squad);
        }
        public static string GetLoggedInUserSquadTitle(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirstValue(CustomClaimType.SquadTitle);
        }
        public static string GetLoggedInUserSquadCounts(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirstValue(CustomClaimType.SquadCounts);
        }
        public static List<string> GetRoles(this ClaimsPrincipal principal)
        {
            return principal.FindAll(ClaimTypes.Role).Select(s => s.Value).ToList();
        }
    }
}