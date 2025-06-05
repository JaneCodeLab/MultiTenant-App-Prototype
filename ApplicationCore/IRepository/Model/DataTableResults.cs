
namespace ApplicationCore;

public record DataTableResults<T>(int draw, int recordsTotal, int recordsFiltered, List<T> data);
