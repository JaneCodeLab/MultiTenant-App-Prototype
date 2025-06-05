
namespace ApplicationCore.DomainModel;

public enum ParameterTypes
{
    [RelatedEnum(EnumType = typeof(ApiRequestType))]
    ApiRequestType = 1,
    [RelatedEnum(EnumType = typeof(ApiRequestStatus))]
    ApiRequestStatus = 2,
    [RelatedEnum(EnumType = typeof(Gender))]
    Gender = 3,
    [RelatedEnum(EnumType = typeof(CrudType))]
    CrudType = 4,
    [RelatedEnum(EnumType = typeof(Priority))]
    Priority = 5,
    [RelatedEnum(EnumType = typeof(ProgressStatus))]
    ProgressStatus = 6,
    [RelatedEnum(EnumType = typeof(TaskType))]
    TaskType = 7,
}