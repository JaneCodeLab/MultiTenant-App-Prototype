
namespace ApplicationCore.DomainModel;

public enum CrudType
{
    Get,
    GetList,
    Create,
    Update,
    CustomUpdate,
    Delete,
    SoftDelete,
    RollBackSoftDelete,
    Active,
    Inactive,
    Lock,
    Unlock,
}