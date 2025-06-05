
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.Helpers;
using Infrastructure.SqlServerAdapter;
using LinqKit;
using System.Linq.Expressions;

namespace ApplicationService;

public class SprintTaskService : BaseService<SprintTask, Guid, TenantDbContext>, ISprintTaskService
{
    private readonly ISprintService _sprintService;
    public SprintTaskService(IUnitOfWork<TenantDbContext> unitOfWork, ISprintService sprintService) : base(unitOfWork)
    {
        _sprintService = sprintService;
    }

    public async Task<ServiceResult> AddToCurrentSprint(Guid id, SysCustomUser user)
    {
        var task = await _repository.FirstAsync(c => c.Id == id, GetIncludes());
        if (task == null)
            return GetFailedResult(user.Language);

        var currentSprintId = await _sprintService.GetCurrentSprintIdAsync(task.Sprint.DepartmentId);
        if (currentSprintId == null)
            return GetFailedResult(user.Language);

        var model = new SprintTask
        {
            Id = id,
            SprintId = currentSprintId ?? task.SprintId
        };

        if (await IsLockedAsync(model.Id))
            return GetLockedResult(user.Language);

        var UpdatableFields = new List<Expression<Func<SprintTask, object>>>() { c => c.SprintId };
        _repository.Update(model, UpdatableFields, user);
        return await SaveAsync(model, user, CrudType.CustomUpdate, UpdatableFields);
    }

    public async Task<ServiceResult> AddToBacklog(Guid id, SysCustomUser user)
    {
        var task = await _repository.FirstAsync(c => c.Id == id, GetIncludes());
        if (task == null)
            return GetFailedResult(user.Language);

        var currentSprintId = await _sprintService.GetBacklogIdAsync(task.Sprint.DepartmentId);
        if (currentSprintId == null)
            return GetFailedResult(user.Language);

        var model = new SprintTask
        {
            Id = id,
            SprintId = currentSprintId ?? task.SprintId
        };

        if (await IsLockedAsync(model.Id))
            return GetLockedResult(user.Language);

        var UpdatableFields = new List<Expression<Func<SprintTask, object>>>() { c => c.SprintId };
        _repository.Update(model, UpdatableFields, user);
        return await SaveAsync(model, user, CrudType.CustomUpdate, UpdatableFields);
    }

    public async Task<ICollection<SprintTask>> GetBacklogItemsAsync(int backlogId)
    {
        var predicate = PredicateBuilder.New<SprintTask>(true);
        predicate.And(c => c.SprintId == backlogId && c.IssueId == null);
        return await _repository.GetListAsync(predicate, GetIncludes());
    }

    public async Task<ICollection<SprintTask>> GetSprintPlanItemsAsync(int sprintId)
    {
        var predicate = PredicateBuilder.New<SprintTask>(true);
        predicate.And(c => c.SprintId == sprintId && c.IssueId == null);
        return await _repository.GetListAsync(predicate, GetIncludes());
    }

    public async Task<ICollection<SprintTask>> GetUsersTasksWithParentAsync(string userId)
    {
        return await _repository.GetListAsync(c => c.Status != ProgressStatus.Done
                                                && c.Status != ProgressStatus.Cancel
                                                && c.Sprint.Current
                                                && c.Assignees.Any(a => a.UserId == userId));
    }

    public async Task<ICollection<SprintTask>> GetUsersCurrentTasksAsync(string userId, int departmentId)
    {
        return await _repository.GetListAsync(c => c.Sprint.Current &&
                                                   c.Sprint.DepartmentId == departmentId &&
                                                   c.Assignees.Any(a => a.UserId == userId), GetIncludes());
    }

    public async Task<ICollection<SprintTask>> GetUsersSprintsTasksAsync(string userId, int sprintId) =>
        await _repository.GetListAsync(c => c.SprintId == sprintId && c.Assignees.Any(a => a.UserId == userId), GetIncludes());

    public override Task<SprintTask?> GetAsync(Guid id)
    {
        return _repository.FirstAsync(c => c.Id == id, GetIncludes());
    }

    public override async Task<ServiceResult> CreateAsync(SprintTask model, SysCustomUser user)
    {
        model.Description = StringCompressor.Compress(model.Description);
        return await base.CreateAsync(model, user);
    }

    public async Task<ServiceResult> CustomUpdateAsync(SprintTask inputModel, SysCustomUser user)
    {
        try
        {
            var model = new SprintTask
            {
                Id = inputModel.Id,
                Title = inputModel.Title,
                Description = StringCompressor.Compress(inputModel.Description),
                Code = inputModel.Code,
                Priority = inputModel.Priority,
                Status = inputModel.Status,
                TaskType = inputModel.TaskType,
                ProjectId = inputModel.ProjectId,
                Deadline = inputModel.Deadline,
                Estimate = inputModel.Estimate,
            };

            var duplicationResult = await IsDuplicated(model, CrudType.Update);
            if (duplicationResult.IsDuplicated)
                return GetDuplicateMessage(user.Language, duplicationResult);

            if (await IsLockedAsync(model.Id))
                return GetLockedResult(user.Language);

            var UpdatableFields = new List<Expression<Func<SprintTask, object>>>() {
                                                                                       c => c.Title,
                                                                                       c => c.Description,
                                                                                       c => c.Code,
                                                                                       c => c.Priority,
                                                                                       c => c.Status,
                                                                                       c => c.TaskType,
                                                                                       c => c.ProjectId,
                                                                                       c => c.Deadline,
                                                                                       c => c.Estimate,
                                                                                     };
            _repository.Update(model, UpdatableFields, user);
            return await SaveAsync(model, user, CrudType.CustomUpdate, UpdatableFields);
        }
        catch (Exception ex)
        {
            return GetFailedResult(user.Language);
        }
    }

    public async Task<ServiceResult> UpdateTimeAsync(SprintTask inputModel, SysCustomUser user)
    {
        try
        {
            var model = new SprintTask
            {
                Id = inputModel.Id,
                Remained = inputModel.Remained,
                Completed = inputModel.Completed,
            };

            if (await IsLockedAsync(model.Id))
                return GetLockedResult(user.Language);

            var UpdatableFields = new List<Expression<Func<SprintTask, object>>>() {
                                                                                       c => c.Remained,
                                                                                       c => c.Completed,
                                                                                     };
            _repository.Update(model, UpdatableFields, user);
            return await SaveAsync(model, user, CrudType.CustomUpdate, UpdatableFields);
        }
        catch (Exception ex)
        {
            return GetFailedResult(user.Language);
        }
    }

    public async Task<ServiceResult> ChangeProressStatus(Guid id, ProgressStatus progressStatus, SysCustomUser user)
    {
        var model = new SprintTask
        {
            Id = id,
            Status = progressStatus,
        };

        if (await IsLockedAsync(model.Id))
            return GetLockedResult(user.Language);

        var UpdatableFields = new List<Expression<Func<SprintTask, object>>>() { c => c.Status };
        _repository.Update(model, UpdatableFields, user);
        return await SaveAsync(model, user, CrudType.CustomUpdate, UpdatableFields);
    }

    public override List<string> GetIncludes() => new() { nameof(SprintTask.Project), nameof(SprintTask.Assignees), nameof(SprintTask.Sprint) };

    public ExpressionStarter<SprintTask> GetPredicate(CrudType ctype, Guid id) => (ctype == CrudType.Update || ctype == CrudType.CustomUpdate) ? PredicateBuilder.New<SprintTask>(c => c.Id != id) : PredicateBuilder.New<SprintTask>();
}