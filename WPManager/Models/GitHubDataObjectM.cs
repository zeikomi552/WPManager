using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPManager.Models
{
    public class GitHubDataObjectM : BindableBase
    {
        #region 検索開始日
        /// <summary>
        /// 検索開始日
        /// </summary>
        DateTime _SearchFrom = DateTime.Today;
        /// <summary>
        /// 検索開始日
        /// </summary>
        public DateTime SearchFrom
        {
            get
            {
                return _SearchFrom;
            }
            set
            {
                if (!_SearchFrom.Equals(value))
                {
                    _SearchFrom = value;
                    RaisePropertyChanged("SearchFrom");
                }
            }
        }
        #endregion

        #region 検索終了日
        /// <summary>
        /// 検索終了日
        /// </summary>
        DateTime _SearchTo = DateTime.Today;
        /// <summary>
        /// 検索終了日
        /// </summary>
        public DateTime SearchTo
        {
            get
            {
                return _SearchTo;
            }
            set
            {
                if (!_SearchTo.Equals(value))
                {
                    _SearchTo = value;
                    RaisePropertyChanged("SearchTo");
                }
            }
        }
        #endregion

        #region 選択している言語
        /// <summary>
        /// 選択している言語
        /// </summary>
        Language? _SelectedLanguage = null;
        /// <summary>
        /// 選択している言語
        /// </summary>
        public Language? SelectedLanguage
        {
            get
            {
                return _SelectedLanguage;
            }
            set
            {
                if (!_SelectedLanguage.Equals(value))
                {
                    _SelectedLanguage = value;
                    RaisePropertyChanged("SelectedLanguage");
                }
            }
        }
        #endregion

    }
}
