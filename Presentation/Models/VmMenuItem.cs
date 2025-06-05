
using System.ComponentModel;

namespace Presentation
{
    public class VmMenuItem
    {
        public VmMenuItem()
        {
            Children = new List<VmMenuItem>();
        }

        public int Order { get; set; }

        public string Title { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Url { get; set; }

        public string Icon { get; set; }

        public string Badge { get; set; }

        public string BadgeClass { get; set; }

        public bool Selected { get; set; }

        public List<VmMenuItem> Children { get; set; }
    }
}
