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
            regionManager.RegisterViewWithRegion("GitHubRegion", typeof(ucGitHubV));
            regionManager.RegisterViewWithRegion("WordpressRegion", typeof(ucWordpressV));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
