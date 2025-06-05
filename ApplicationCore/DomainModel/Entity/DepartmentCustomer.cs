namespace ApplicationCore.DomainModel;

public class DepartmentCustomer : BaseEntity<int>
{
    public int DepartmentId { get; set; }
    public int CustomerId { get; set; }

    public virtual Department? Department { get; set; }
    public virtual Customer? Customer { get; set; }
}