
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.Helpers;

namespace ApplicationService;

public class SysLogMinimalListItem : BaseMinimalListItem<Guid>
{
    private readonly string _userTimeZone;
    public SysLogMinimalListItem()
    {
        _userTimeZone = GeneralVariables.DefaultTimeZone;
    }
    public SysLogMinimalListItem(string userTimeZone)
    {
        _userTimeZone = userTimeZone;
    }


    public string? EntityName { get; set; }

    public string? EntityId { get; set; }

    public CrudType CrudType { get; set; }

    public string CrudTypeText => CrudType.ToString();

    [MainColumn(OrderBy = nameof(SysLog.CreatedAt))]
    public string CreatedAtText => CreatedAt.GetLocalDateTime(_userTimeZone).ToDateTimeStyle();

    public string? UpdatedAtText => UpdatedAt?.GetLocalDateTime(_userTimeZone).ToDateTimeStyle();
}
