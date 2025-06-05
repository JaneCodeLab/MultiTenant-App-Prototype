
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface ISysParameterService : IBaseService<SysParameter, Guid, ApplicationDbContext>
{
    Task<ICollection<DtoSysParameter>> GetAllAsync();
    Task<DtoSysParameter?> GetAsync(Guid id);
    void FetchAllParameters();
    Task<ServiceResult> UpdateEquivalentAsync(Guid id, string equivalent, SysCustomUser user);
    Task CheckMappingsValidity(Language language, bool insertMissedOnes);
}