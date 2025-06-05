namespace Infrastructure.Helpers;

public class TenantConfig
{
    public int TenantId { get; set; }
    public string Title { get; set; }
    public string Logo { get; set; }    
    public string DbUsername { get; set; }
    public string DbPassword { get; set; }
    public string DbIp { get; set; }
}