using ApplicationService;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class SprintReviewV3Controller : BaseController
    {
        private readonly ILogger<SprintReviewV3Controller> _logger;
        private readonly IActivityService _activityService;
        private readonly ISprintTaskService _sprintTaskService;
        private readonly IDepartmentMemberService _departmentMemberService;
        private readonly IDepartmentService _departmentService;

        public SprintReviewV3Controller(ILogger<SprintReviewV3Controller> logger,
                                 IDataProtectionProvider dataProtectionProvider,
                                 IDepartmentService departmentService,
                                 IDepartmentMemberService departmentMemberService,
                                 IActivityService activityService,
                                 ISprintTaskService sprintTaskService)
                            : base(dataProtectionProvider)
        {
            _logger = logger;
            _activityService = activityService;
            _sprintTaskService = sprintTaskService;
            _departmentMemberService = departmentMemberService;
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index(string userId)
        {
            var departmentId = (User.GetLoggedInUserSquad()).ToInt();
            var members = await _departmentMemberService.GetDepartmentMembers(departmentId);

            if (userId.IsNullOrEmpty() || !members.Any(c => c.UserId == userId))
                userId = members.FirstOrDefault()?.UserId;

            ViewData["Members"] = members.Select(s => new
            {
                s.UserId,
                ProfileImage = $"/img/user/general/{s.ProfileImage}",
                s.FullName,
                selected = false
            }
            );
            ViewBag.DepartmentId = departmentId;
            ViewBag.CurrentMember = userId;
            ViewBag.CurrentMemberFullName = members.FirstOrDefault(c => c.UserId == userId)?.FullName;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetSprintDetail(string userId)
        {
            var departmentId = (User.GetLoggedInUserSquad()).ToInt();
            var detail = await _sprintTaskService.GetUsersCurrentTasksAsync(userId, departmentId);
            foreach (var task in detail)
                task.Description = StringCompressor.Decompress(task.Description);
            return Ok(detail);
        }
    }
}
