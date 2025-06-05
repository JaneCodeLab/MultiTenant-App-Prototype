
namespace ApplicationCore.DomainModel;

public class SysApiLog : BaseEntity<Guid>
{
    public SysApiLog() => Id = Guid.NewGuid();

    public int ApiRequestTypeParamId { get; set; }
    public int ApiRequestStatusParamId { get; set; }
    public string RequestIP { get; set; } = string.Empty;
    public string RequestURL { get; set; } = string.Empty;
    public DateTime RequestDateTime { get; set; }
    public DateTime ResponseDateTime { get; set; }
    public int Duration { get; set; } = 0;
    public string ResponseBodyCompressed { get; set; } = string.Empty;
    public string RequestBodyCompressed { get; set; } = string.Empty;
}