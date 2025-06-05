
namespace ApplicationCore.DomainModel;

public enum SysMenuExpression
{
    SystemContents = 1,
    SystemLogs = 2,
    SystemSettings = 3,
    Settings = 4,
    DepartmentSettings = 5,
    Scrum = 6,
    Report = 7,
    MyPanel = 8,
    SystemLanguage = 9,

    Expressions = 100,
    Faq = 101,
    Help = 102,
    ReleaseNote = 103,

    Bugs = 200,
    AuditLogs = 201,
    ApiLogs = 202,
    TenantLogs = 203,

    Parameters = 300,
    ClearCaches = 301,
    Users = 302,
    Roles = 303,
    UserTenants = 304,
    Employees = 305,

    Customers = 400,
    Departments = 401,
    Personel = 402,
    Projects = 403,
    
    Members = 500,
    DepartmentCustomers = 501,
    DepartmentRoles = 502,

    Sprints = 600,
    Backlog = 601,
    CurrentSprint = 602,

    PersonelActivities = 700,
    SprintReview = 701,
    SprintReviewReport = 702,
    SprintReviewV3 = 703,

    MyActivities = 800,
    MyTasks = 801
}