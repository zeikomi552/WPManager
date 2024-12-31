using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPManager.Models;

namespace WPManager.ViewModels.UserControls
{
    public class GitHubBaseViewModel : BindableBase
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

        #region GitHub接続用パラメーター
        /// <summary>
        /// GitHub接続用パラメーター
        /// </summary>
        GitHubParameterM _GitHubParameter = new GitHubParameterM();
        /// <summary>
        /// GitHub接続用パラメーター
        /// </summary>
        public GitHubParameterM GitHubParameter
        {
            get
            {
                return _GitHubParameter;
            }
            set
            {
                if (_GitHubParameter == null || !_GitHubParameter.Equals(value))
                {
                    _GitHubParameter = value;
                    RaisePropertyChanged("GitHubParameter");
                }
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="gConfig"></param>
        public GitHubBaseViewModel(IGlobalConfigM gConfig)
        {
            this.WPParameter = gConfig.WPConfig;
        }
        #endregion
    }
}
