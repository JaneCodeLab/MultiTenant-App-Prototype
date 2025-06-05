
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.DomainModel;

public enum SprintTaskExpression
{
    SprintTask = 1,
    SprintTasks = 2,

    Hours = 10,
    Minutes = 11,
    Seconds = 12,

    Create = 13,
    Edit = 14,
    Delete = 15,
    AddAssignees = 16,

    Title = 19,
    Code = 20,
    SequenceNo = 21,
    Description = 22,
    Priority = 23,
    Status = 24,
    TaskType = 25,
    Deadline = 26,
    FinishDate = 27,
    Remained = 28,
    Estimate = 29,
    Completed = 30,
    DepartmentId = 31,
    SprintId = 32,
    ProjectId = 33,
    IssueId = 34,
    Assignees = 35,
}