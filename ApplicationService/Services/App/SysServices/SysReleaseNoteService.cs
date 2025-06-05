
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;
using System.Linq.Expressions;

namespace ApplicationService;

public class SysReleaseNoteService : BaseService<SysReleaseNote, int, ApplicationDbContext>, ISysReleaseNoteService
{
    public SysReleaseNoteService(IUnitOfWork<ApplicationDbContext> unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<string?> GetLatestVersionNo()
    {
        var order = new OrderBy<SysReleaseNote> { Orders = new List<Expression<Func<SysReleaseNote, object>>> { c => c.ReleaseDate }, OrderType = OrderType.Desc };
        var releaseList = await _repository.GetListAsync(c => !c.IsComing, order, s => new SysReleaseNote { ReleaseNo = s.ReleaseNo, ReleaseDate = s.ReleaseDate });
        return releaseList?.FirstOrDefault()?.ReleaseNo;
    }

}