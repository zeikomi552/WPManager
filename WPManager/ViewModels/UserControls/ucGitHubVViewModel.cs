using Markdig;
using Octokit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPManager.Models;
using WPManager.Common.Extensions;
using WPManager.Models.GitHub;

namespace WPManager.ViewModels.UserControls
{
    public class ucGitHubVViewModel : BindableBase
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

        #region ブログマネージャー
        /// <summary>
        /// ブログマネージャー
        /// </summary>
        GitHubBlogManagerM _BlogManager = new GitHubBlogManagerM();
        /// <summary>
        /// ブログマネージャー
        /// </summary>
        public GitHubBlogManagerM BlogManager
        {
            get
            {
                return _BlogManager;
            }
            set
            {
                if (_BlogManager == null || !_BlogManager.Equals(value))
                {
                    _BlogManager = value;
                    RaisePropertyChanged("BlogManager");
                }
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="gConfig"></param>
        public ucGitHubVViewModel(IGlobalConfigM gConfig)
        {
            this.WPParameter = gConfig.WPConfig;
            this.BlogManager.GitHubParameter = gConfig.GitHubConfig!;
        }
        #endregion

        #region 検索処理
        /// <summary>
        /// 検索処理
        /// </summary>
        public void Search()
        {
            this.BlogManager.Search(0);
        }
        #endregion

        #region 言語選択の解除処理
        /// <summary>
        /// 言語選択の解除処理
        /// </summary>
        public void LanguageClear()
        {
            try
            {
                this.BlogManager.SelectedLanguage = null;
            }
            catch
            {

            }
        }
        #endregion

        #region 記事投稿処理
        /// <summary>
        /// 記事投稿処理
        /// </summary>
        public void Post()
        {
            try
            {
                this.BlogManager.Post(this.WPParameter!);
            }
            catch
            {

            }
        }
        #endregion
    }
}
