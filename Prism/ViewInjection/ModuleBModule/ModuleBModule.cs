using ModuleB.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace ModuleB
{
    public class ModuleBModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public ModuleBModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            IRegion region = _regionManager.Regions["ContentRegionB"];

            var view1 = containerProvider.Resolve<ViewB>();
            region.Add(view1);

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
