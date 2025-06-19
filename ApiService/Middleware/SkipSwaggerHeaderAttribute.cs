namespace ApiService;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class SkipSwaggerHeaderAttribute : Attribute
{
}
