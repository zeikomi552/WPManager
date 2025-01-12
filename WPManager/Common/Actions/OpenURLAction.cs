using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPManager.Common.Actions
{
    #region URLをブラウザで開くアクション 
    /// <summary> 
    /// URLをブラウザで開くアクション 
    /// </summary> 
    public class OpenURLAction : TriggerAction<FrameworkElement>
    {
        public enum SearchEngineEnum
        {
            None,
            Google,
            Bing
        }

        public static readonly DependencyProperty SearchEngineProperty =
        DependencyProperty.Register("SearchEngine", typeof(SearchEngineEnum), typeof(OpenURLAction), new UIPropertyMetadata());

        public SearchEngineEnum SearchEngine
        {
            get { return (SearchEngineEnum)GetValue(SearchEngineProperty); }
            set { SetValue(SearchEngineProperty, value); }
        }


        public static readonly DependencyProperty URLProperty =
        DependencyProperty.Register("URL", typeof(string), typeof(OpenURLAction), new UIPropertyMetadata());

        public string URL
        {
            get { return (string)GetValue(URLProperty); }
            set { SetValue(URLProperty, value); }
        }

        protected override void Invoke(object obj)
        {
            try
            {
                switch (SearchEngine)
                {
                    case SearchEngineEnum.None:
                        {
                            var startInfo = new System.Diagnostics.ProcessStartInfo(URL);
                            startInfo.UseShellExecute = true;
                            System.Diagnostics.Process.Start(startInfo);
                            break;
                        }
                    case SearchEngineEnum.Google:
                        {
                            var startInfo = new System.Diagnostics.ProcessStartInfo("https://www.google.co.jp/search?q=" + URL);
                            startInfo.UseShellExecute = true;
                            System.Diagnostics.Process.Start(startInfo);
                            break;
                        }
                    case SearchEngineEnum.Bing:
                        {
                            var startInfo = new System.Diagnostics.ProcessStartInfo("https://www.bing.com/search?pglt=931&q=" + URL);
                            startInfo.UseShellExecute = true;
                            System.Diagnostics.Process.Start(startInfo);
                            break;
                        }
                }

            }
            catch// (Exception ex)
            {
                //ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
    }
    #endregion
}
