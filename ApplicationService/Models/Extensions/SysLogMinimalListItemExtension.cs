
using ApplicationCore.DomainModel;

namespace ApplicationService;

public static class SysLogMinimalListItemExtension
{
    public static SysLogMinimalListItem ToDto(this SysLog s, string userTimeZone) =>
        new SysLogMinimalListItem(userTimeZone)
        {
            Id = s.Id,
            Active = s.Active,
            Locked = s.Locked,
            Owner = s.Owner,
            CreatedAt = s.CreatedAt,
            CreatedBy = s.CreatedBy,
            UpdatedAt = s.UpdatedAt,
            UpdatedBy = s.UpdatedBy,

            CrudType = s.CrudType,
            EntityId = s.EntityId,
            EntityName = s.EntityName
        };
}
