
namespace ApplicationService;

public interface IBacklogService
{
    Task<ICollection<BacklogMinimalListItem>> GetBacklogItems(int departmentId);
}