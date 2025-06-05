
using ApplicationCore.DomainModel;
using Infrastructure.Helpers;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface ISysExpressionService : IBaseService<SysExpression, Guid, ApplicationDbContext>
{
    Task<ICollection<DtoSysExpression>> GetAllAsync();
    Task<DtoSysExpression?> GetAsync(Guid id);
    void FetchAllExpressions();
    Task<ServiceResult> UpdateEquivalentAsync(Guid id, string equivalent, SysCustomUser user);
    Task CheckMappingsValidity(Language language, bool insertMissedOnes);
}