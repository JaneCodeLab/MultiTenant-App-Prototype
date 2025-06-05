
namespace ApplicationCore.DomainModel;

public enum SysCustomUserExpression
{
    CustomUser = 11000,
    CustomUsers = 11001,
    CustomUser_Create = 11002,
    CustomUser_Edit = 11003,
    CustomUser_Delete = 11004,

    CustomUser_FirstName = 11010,
    CustomUser_LastName = 11011,
    CustomUser_Email = 11012,
    CustomUser_EmailConfirmed = 11013,
    CustomUser_PhoneNumber = 11014,
    CustomUser_PhoneNumberConfirmed = 11015,
    CustomUser_AccessFailedCount = 11016,
    CustomUser_Gender = 11017,
    CustomUser_Language = 11018,
    CustomUser_IsEmployeeUser = 11019,

    CustomUser_AuthenticatorCode = 11050,
    CustomUser_2FGeneralMessage = 11051,
    CustomUser_InvalidACMessage = 11052,
    CustomUser_SigninDifferentUser = 11054,
    CustomUser_2FEmailSubject = 11055,
    CustomUser_2FEmailBody = 11056,
    CustomUser_2FEmailGreeting = 11057,
    CustomUser_2FEmailSignature = 11058,
    CustomUser_ResetPasswordSubject = 11059,
    CustomUser_ResetPasswordEmailBody = 11060,
    CustomUser_ActivationEmailBody = 11061,
    CustomUser_ActivationSubject = 11062,
    CustomUser_EmailExist = 11063,
    CustomUser_CheckYourEmailForResetPassword = 11064,
    CustomUser_PasswordHasBeenReset = 11065,
    CustomUser_AddRoleToUser = 11066,
    CustomUser_SelectUserForRelatedTenants = 11067,
}