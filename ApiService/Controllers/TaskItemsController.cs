using ApiService.Constants;
using ApplicationService;
using ApplicationService.Dto;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiService.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route(RouteConstants.Tasks)]
    [Produces(ApiMediaTypes.Json)]
    [ApiExplorerSettings(GroupName = "v1")]
    public class TaskItemsController : BaseController
    {
        private readonly ILogger<TaskItemsController> _logger;
        private readonly ITaskApiService _taskApiService;
        public TaskItemsController(ITaskApiService taskApiService, ILogger<TaskItemsController> logger)
        {
            _taskApiService = taskApiService ?? throw new ArgumentNullException(nameof(taskApiService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        /// <summary>
        /// Retrieves all task items for the current tenant.
        /// </summary>
        /// <returns>A list of task items.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<TaskItemDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            var result = await _taskApiService.GetAllAsync(TenantId);
            return FromApiResponse(result);
        }

        /// <summary>
        /// Retrieves a task item by ID for the current tenant.
        /// </summary>
        [HttpGet(RouteConstants.TaskById)]
        [ProducesResponseType(typeof(ApiResponse<TaskItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _taskApiService.GetByIdAsync(TenantId, id);
            return FromApiResponse(result);
        }
    }
}
