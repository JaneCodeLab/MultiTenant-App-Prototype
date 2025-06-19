using ApplicationService.Dto;

namespace ApplicationService;

public interface ITaskApiService : IBaseApiService
{
    /// <summary>
    /// Retrieves all task items for a tenant.
    /// </summary>
    /// <returns>A list of task items wrapped in an ApiResponse.</returns>
    Task<ApiResponse<List<TaskItemDto>>> GetAllAsync(int tenantId);

    /// <summary>
    /// Retrieves a task item by ID for a tenant.
    /// </summary>
    /// <returns>The task item wrapped in an ApiResponse.</returns>
    Task<ApiResponse<TaskItemDto>> GetByIdAsync(int tenantId, Guid id);
}