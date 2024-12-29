using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPManager.Views.UserControls;

namespace WPManager.Common
{
    public class Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            IRegion region = regionManager.Regions["ContentRegion"];
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(ucWordpressV));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
