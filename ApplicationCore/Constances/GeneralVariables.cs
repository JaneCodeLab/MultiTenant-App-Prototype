
namespace ApplicationCore.DomainModel;

public static class GeneralVariables
{
    public static string ApplicationName = "PMA";
    public static string ApplicationBrandName = "PM Assistant";
    public static bool ActiveRedisCache = false;
    public static string LatestVersionNo = "1.0.1";
    public static SysCustomUser SystemUser => new() { FirstName = UserType.System, Language = Language.English };
    public static string DefaultTimeZone = "Central Standard Time";
    public static string MainBranch = "Center";
}