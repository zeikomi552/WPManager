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
        #region Wordpress接続用パラメーター
        /// <summary>
        /// Wordpress接続用パラメーター
        /// </summary>
        IWPParameterM? _WPParameter;
        /// <summary>
        /// Wordpress接続用パラメーター
        /// </summary>
        public IWPParameterM? WPParameter
        {
            get
            {
                return _WPParameter;
            }
            set
            {
                if (_WPParameter == null || !_WPParameter.Equals(value))
                {
                    _WPParameter = value;
                    RaisePropertyChanged("WPParameter");
                }
            }
        }
        #endregion

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



        public MainWindowViewModel(IWPParameterM wpParameter, IGlobalConfigM gConfig)
        {
            this.GlobalConfig = gConfig;
            this.WPParameter = wpParameter;
            this.GlobalConfig.Load();
        }

    }
}
