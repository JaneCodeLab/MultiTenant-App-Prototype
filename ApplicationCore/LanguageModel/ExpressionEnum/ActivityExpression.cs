
namespace ApplicationCore.DomainModel;

public enum ActivityExpression
{
    SprintTask = 1,
    SprintTasks = 2,

    Create = 13,
    Edit = 14,
    Delete = 15,

    Description = 20,
    StartDate = 21,
    EndDate = 22,
    Duration = 23,
    SprintTaskId = 24,
}