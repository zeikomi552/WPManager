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
    public class ucGitHubVViewModel : GitHubBaseViewModel
    {
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
        public ucGitHubVViewModel(IGlobalConfigM gConfig) : base(gConfig)
        {
            this.BlogManager.GitHubParameter = gConfig.GitHubConfig!;
        }
        #endregion

        #region 検索処理
        /// <summary>
        /// 検索処理
        /// </summary>
        public void Search()
        {
            this.BlogManager.Search();
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
                this.BlogManager.SearchCondition.SelectedLanguage = null;
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
