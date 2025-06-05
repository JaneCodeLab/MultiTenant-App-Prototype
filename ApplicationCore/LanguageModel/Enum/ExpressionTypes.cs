
namespace ApplicationCore.DomainModel;

public enum ExpressionTypes
{
    [RelatedEnum(EnumType = typeof(SysApiLogExpression))]
    SysApiLog = 1,
    [RelatedEnum(EnumType = typeof(SysApiUserExpression))]
    SysApiUser = 2,
    [RelatedEnum(EnumType = typeof(SysCustomUserExpression))]
    SysCustomUser = 3,
    [RelatedEnum(EnumType = typeof(SysExceptionExpression))]
    SysException = 4,
    [RelatedEnum(EnumType = typeof(SysFaqExpression))]
    SysFaq = 5,
    [RelatedEnum(EnumType = typeof(SysHelpExpression))]
    SysHelp = 6,
    [RelatedEnum(EnumType = typeof(SysLogExpression))]
    SysLog = 7,
    [RelatedEnum(EnumType = typeof(SysParameterExpression))]
    SysParameter = 8,
    [RelatedEnum(EnumType = typeof(SysReleaseNoteExpression))]
    SysReleaseNote = 9,
    [RelatedEnum(EnumType = typeof(ApiResponseExpression))]
    ApiResponse = 10,
    [RelatedEnum(EnumType = typeof(SysMenuExpression))]
    SysMenu = 11,
    [RelatedEnum(EnumType = typeof(BaseEntityExpression))]
    BaseEntity = 12,
    [RelatedEnum(EnumType = typeof(GeneralExpression))]
    General = 13,

    [RelatedEnum(EnumType = typeof(DepartmentExpression))]
    Department = 100,
    [RelatedEnum(EnumType = typeof(CustomerExpression))]
    Customer = 101,
    [RelatedEnum(EnumType = typeof(ProjectExpression))]
    Project = 102,
    [RelatedEnum(EnumType = typeof(DepartmentCustomerExpression))]
    DepartmentCustomer = 103,
    [RelatedEnum(EnumType = typeof(SprintExpression))]
    Sprint = 104,
    [RelatedEnum(EnumType = typeof(DepartmentMemberExpression))]
    DepartmentMember = 105,
    [RelatedEnum(EnumType = typeof(DepartmentRoleExpression))]
    DepartmentRole = 106,
    [RelatedEnum(EnumType = typeof(TaskAssigneeExpression))]
    TaskAssignee = 107,
    [RelatedEnum(EnumType = typeof(SprintTaskExpression))]
    SprintTask = 108,
    [RelatedEnum(EnumType = typeof(ActivityExpression))]
    Activity = 109,
    [RelatedEnum(EnumType = typeof(IssueExpression))]
    Issue = 110,
    [RelatedEnum(EnumType = typeof(BacklogExpression))]
    Backlog = 111,
    [RelatedEnum(EnumType = typeof(MyActivitiesExpression))]
    MyActivities = 112,
    [RelatedEnum(EnumType = typeof(CurrentSprintExpression))]
    CurrentSprint = 113,    
    [RelatedEnum(EnumType = typeof(SprintReviewExpression))]
    SprintReview = 114,
}