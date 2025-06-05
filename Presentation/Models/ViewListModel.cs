
namespace Presentation
{
    public class ViewListModel<TModel, TFilter>
    {
        public TFilter Filter { get; set; }
        public ICollection<TModel> Records { get; set; }
    }

    public record ViewListModel<TFilter>(TFilter Filter);
}
