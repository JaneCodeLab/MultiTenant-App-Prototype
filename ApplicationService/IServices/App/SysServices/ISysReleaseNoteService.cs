
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface ISysReleaseNoteService : IBaseService<SysReleaseNote, int, ApplicationDbContext>
{
    Task<string> GetLatestVersionNo();
}