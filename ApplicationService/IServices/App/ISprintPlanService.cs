
namespace ApplicationService;

public interface ISprintPlanService
{
    Task<ICollection<SprintPlanMinimalListItem>> GetSprintPlanItems(int sprintId);
}