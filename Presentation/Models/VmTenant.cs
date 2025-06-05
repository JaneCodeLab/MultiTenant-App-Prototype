namespace Presentation;
public class VmTenant
{
    public int TenantId { get; set; }
    public string Title { get; set; }
    public string Logo { get; set; }
    public bool Active { get; set; }
    public int UserCount { get; set; }
    public int DepartmentCount { get; set; }
    public int CustomerCount { get; set; }
    public int ProjectCount { get; set; }
    public int SprintCount { get; set; }
    public int SprintTaskCount { get; set; }
    public int LogCount { get; set; }
    public int ApiLogCount { get; set; }
}
