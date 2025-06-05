
namespace ApplicationCore.DomainModel;

public enum ApiResponseExpression
{
    ApiResponse_Success = 5000,
    ApiResponse_Nothing_Done = 5001,
    ApiResponse_Unexpected_Error = 5002,
    ApiResponse_Invalid_Tenant = 5003,
    ApiResponse_Null_Input = 5004,
    ApiResponse_Invalid_Input = 5005,
    ApiResponse_Unknown_Problem = 5006,

    ApiResponse_Not_Found = 5020,

    ApiResponse_Invalid_Credential = 5050,
    ApiResponse_Duplicate_Register = 5051,
    ApiResponse_Customer_Is_Locked = 5052,
    ApiResponse_Invalid_Old_Password = 5053,
    ApiResponse_Password_Has_Been_Changed = 5054,
    ApiResponse_Email_Does_Not_Exist = 5055,
    ApiResponse_Forgot_Password_Email_Subject = 5056,
    ApiResponse_Forgot_Password_Email_Body = 5057,
    ApiResponse_Invalid_Reset_Password_Token = 5058,

}