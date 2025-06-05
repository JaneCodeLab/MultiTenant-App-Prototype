
namespace ApplicationCore.DomainModel;

public enum BaseEntityExpression
{
    CreatedAt = 20000,
    CreatedBy = 20001,
    UpdatedAt = 20002,
    UpdatedBy = 20003,
    Active = 20004,
    Inactive = 20005,
    CreatedAtStart = 20006,
    CreatedAtEnd = 20007,
    UpdatedAtStart = 20008,
    UpdatedAtEnd = 20009,
    Locked = 20010,
    Id = 20011,
    IsDeleted = 20012,
    Owner = 20013,
    CreatedById = 20014,
    UpdatedById = 20015,

    Requiered_Validation_Message = 20020,
    Duplicate_Validation_Message = 20021,
    Locked_Validation_Message = 20022,
    Success_Message = 20023,
    Failed_Message = 20024,
    Failed_Message_With_Ref = 20025,
    Failed_Delete_Message = 20026,
}