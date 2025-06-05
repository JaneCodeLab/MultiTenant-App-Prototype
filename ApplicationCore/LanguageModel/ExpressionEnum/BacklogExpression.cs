
namespace ApplicationCore.DomainModel;

public enum BacklogExpression
{
    Backlog = 1,
    Backlogs = 2,

    Create = 13,
    Edit = 14,
    Delete = 15,
    AddToCurrentSprint = 16,
    Sure_To_Add_To_Current_Sprint = 17, 
    Yes_I_Am_Sure_To_Add_To_Current_Sprint = 18,

    Title = 20,
    Code = 21,
    Description = 22,
    Priority = 23,
    Status = 24,
    Deadline = 25,
    ProjectId = 26,
    DepartmentId = 27,
    IsIssue = 28,
    ProjectTitle = 29,
}