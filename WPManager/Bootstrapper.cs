using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using WPManager.Views;
using WPManager.Common;
using WPManager.Views.UserControls;
using WPManager.ViewModels;
using WPManager.ViewModels.UserControls;
using WPManager.Models;

namespace WPManager
{
    public class Bootstrapper : PrismBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // シングルトンクラスとして登録したい時
            containerRegistry.RegisterSingleton<IWPParameterM?, WPParameterM>();
            containerRegistry.RegisterSingleton<IGlobalConfigM?, GlobalConfigM>();
            //containerRegistry.RegisterDialog<MediaPlayer, MediaPlayerViewModel>();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
            ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
            ViewModelLocationProvider.Register<ucTopMenuV, ucTopMenuVViewModel>();
            ViewModelLocationProvider.Register<ucWordpressV, ucWordpressVViewModel>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<Module>();
        }
    }
}
