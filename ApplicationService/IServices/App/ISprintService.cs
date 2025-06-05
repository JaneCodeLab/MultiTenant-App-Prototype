
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface ISprintService : IBaseService<Sprint, int, TenantDbContext>
{
    Task<ServiceResult> MakeCurrent(int id, int departmentId, SysCustomUser user);
    Task<ICollection<Sprint>> GetDepartmentSprintsAsync(int departmentId);
    Task<ICollection<Sprint>> GetAllSprintsAsync(BaseFilter filter, string sourceTimeZoneId);
    Task<int?> GetBacklogIdAsync(int departmentId);
    Task<int?> GetCurrentSprintIdAsync(int departmentId);
    Task<ServiceResult> CustomUpdateAsync(Sprint inputModel, SysCustomUser user);
    Task<ServiceResult> CreateBacklogAsync(int departmentId);
}