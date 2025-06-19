using ApplicationCore.DomainModel;
using ApplicationService.Constants;
using ApplicationService.Dto;

namespace ApplicationService;

public class TaskApiService : BaseApiService, ITaskApiService
{
    public async Task<ApiResponse<List<TaskItemDto>>> GetAllAsync(int tenantId)
    {
        if (!VaildateTenantUnitOfWork(tenantId))
            return ApiResponse<List<TaskItemDto>>.Fail(ApiCommonMessages.InvalidTenant);

        var repo = unitOfWork.GetRepository<SprintTask, Guid>(); 
        var tasks = await repo.GetListAsync();

        var result =  tasks.Select(t => new TaskItemDto
        {
            Id = t.Id,
            Title = t.Title,
            IsCompleted = t.Status == ProgressStatus.Done
        }).ToList();

        return ApiResponse<List<TaskItemDto>>.Ok(result, ApiMessages.FetchSuccess("Task Items"));
    }

    public async Task<ApiResponse<TaskItemDto>> GetByIdAsync(int tenantId, Guid id)
    {
        if (!VaildateTenantUnitOfWork(tenantId))
            return ApiResponse<TaskItemDto>.Fail(ApiCommonMessages.InvalidTenant);

        var repo = unitOfWork.GetRepository<SprintTask, Guid>();
        var task = await repo.FindAsync(id);

        if (task == null)
            return ApiResponse<TaskItemDto>.Fail(ApiMessages.NotFound("Task"));

        var dto = new TaskItemDto
        {
            Id = task.Id,
            Title = task.Title,
            IsCompleted = task.Status == ProgressStatus.Done
        };

        return ApiResponse<TaskItemDto>.Ok(dto, ApiMessages.FetchSuccess("Task"));
    }
}