
namespace ApplicationCore
{
    public class Pagination
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public bool TakeAll { get; set; } = false;
        public int SkipCount { get { return (Page - 1) * Size; } }
    }
}
