using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPManager.Models
{
    public class GitHubParameterM : BindableBase
    {
        #region 製品名
        /// <summary>
        /// 製品名
        /// </summary>
        string _ProductName = string.Empty;
        /// <summary>
        /// 製品名
        /// </summary>
        public string ProductName
        {
            get
            {
                return _ProductName;
            }
            set
            {
                if (_ProductName == null || !_ProductName.Equals(value))
                {
                    _ProductName = value;
                    RaisePropertyChanged("ProductName");
                }
            }
        }
        #endregion

        #region アクセストークン
        /// <summary>
        /// アクセストークン
        /// </summary>
        string _AccessToken = string.Empty;
        /// <summary>
        /// アクセストークン
        /// </summary>
        public string AccessToken
        {
            get
            {
                return _AccessToken;
            }
            set
            {
                if (_AccessToken == null || !_AccessToken.Equals(value))
                {
                    _AccessToken = value;
                    RaisePropertyChanged("AccessToken");
                }
            }
        }
        #endregion


    }
}
