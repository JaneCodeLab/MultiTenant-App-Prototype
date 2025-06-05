
using ApplicationCore.DomainModel;

namespace ApplicationCore.DomainService;

public static class CustomUserDomainService
{
    public static string  GetFullName(this SysCustomUser user)
    {
        return $"{user.FirstName} {user.LastName}".Trim();
    }
}