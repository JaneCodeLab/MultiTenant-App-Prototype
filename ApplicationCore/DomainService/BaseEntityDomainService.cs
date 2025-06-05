
using ApplicationCore.DomainModel;

namespace ApplicationCore.DomainService;

public static class BaseEntityDomainService
{
    public static void SetCreatedFileds<BT>(this BaseEntity<BT> model, SysCustomUser onlineUser)
    {
        model.CreatedAt = DateTime.UtcNow;

        if (string.IsNullOrEmpty(model.CreatedBy) && string.IsNullOrEmpty(model.CreatedById))
        {
            model.CreatedById = onlineUser.Id;
            model.CreatedBy = onlineUser.GetFullName();
        }

        if (string.IsNullOrEmpty(model.Owner))
            model.Owner = onlineUser.Id;
    }

    public static void SetUpdatedFileds<BT>(this BaseEntity<BT> model, SysCustomUser onlineUser)
    {
        model.UpdatedAt = DateTime.UtcNow;
        model.UpdatedBy = onlineUser.GetFullName();
        model.UpdatedById = onlineUser.Id;
    }
}