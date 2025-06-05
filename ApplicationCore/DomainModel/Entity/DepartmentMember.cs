namespace ApplicationCore.DomainModel;

public class DepartmentMember : BaseEntity<Guid>
{
    public string FullName { get; set; } = string.Empty;
    public string ProfileImage { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public Guid DepartmentRoleId { get; set; }
    public Guid? SupervisorMemberId { get; set; }

    public virtual DepartmentRole? DepartmentRole { get; set; }
    public virtual DepartmentMember? SupervisorMember { get; set; }
    public virtual ICollection<DepartmentMember>? Subordinates { get; set; }
}