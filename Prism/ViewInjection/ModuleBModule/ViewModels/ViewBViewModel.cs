using Prism.Mvvm;

namespace ModuleB.ViewModels
{
    public class ViewBViewModel : BindableBase
    {
        public string ResponseText { get; set; }

        public ViewBViewModel()
        {
            ResponseText = "Bear ViewB";
        }
    }
}
