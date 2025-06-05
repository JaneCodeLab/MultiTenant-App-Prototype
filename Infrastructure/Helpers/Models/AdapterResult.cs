
using ApplicationCore.DomainModel;

namespace Infrastructure.Helpers
{
    public record AdapterResult<T>(SysApiLog Log, T Result, string? Message = null);
}
