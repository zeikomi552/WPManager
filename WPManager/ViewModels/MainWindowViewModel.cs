using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPManager.Common.Utilites;
using WPManager.Models;

namespace WPManager.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region コンフィグデータ
        /// <summary>
        /// コンフィグデータ
        /// </summary>
        IGlobalConfigM? _GlobalConfig;
        /// <summary>
        /// コンフィグデータ
        /// </summary>
        public IGlobalConfigM? GlobalConfig
        {
            get
            {
                return _GlobalConfig;
            }
            set
            {
                if (_GlobalConfig == null || !_GlobalConfig.Equals(value))
                {
                    _GlobalConfig = value;
                    RaisePropertyChanged("GlobalConfig");
                }
            }
        }
        #endregion

        public MainWindowViewModel(IGlobalConfigM gConfig)
        {
            this.GlobalConfig = gConfig;
            this.GlobalConfig.Load();
        }

        public void Closing()
        {
            try
            {
                this.GlobalConfig!.Save();
            }
            catch
            {

            }
        }

    }
}
