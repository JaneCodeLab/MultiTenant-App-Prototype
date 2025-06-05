
namespace ApplicationCore.IRepository.Model;

public class PagedList<T> : List<T>
{
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => TotalPages > CurrentPage;


    public PagedList(List<T> items, int cout, int pageNumber, int pageSize)
    {
        TotalCount = cout;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling((double)Count / pageSize);
        AddRange(items);
    }
}
