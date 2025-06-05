
namespace ApplicationCore.DomainModel;

public class SysException : BaseEntity<Guid>
{
    public SysException() => Id = Guid.NewGuid();

    public string? ActionName { get; set; }
    public string? ControllerName { get; set; }
    public string? Parameters { get; set; }
    public string? Path { get; set; }
    public string? Method { get; set; }
    public string? Host { get; set; }
    public string? IP { get; set; }
    public string? Scheme { get; set; }
    public string? Language { get; set; }
    public string? StackTrace { get; set; }
    public string? Source { get; set; }
    public string? Message { get; set; }
    public string? Type { get; set; }
    public string? InnerExceptionMessage { get; set; }
    public string? InnerExceptionType { get; set; }
}