
using ApplicationCore.DomainModel;
namespace ApplicationService;

public class DtoSysParameter
{
    public Guid Id { get; set; }
    public Language Language { get; set; }
    public ParameterTypes ParameterType { get; set; }
    public int ParameterItem { get; set; }
    public string? Equivalent { get; set; }
}