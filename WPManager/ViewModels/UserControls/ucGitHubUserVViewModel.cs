using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPManager.Models.GitHub;
using WPManager.Models;

namespace WPManager.ViewModels.UserControls
{
    public class ucGitHubUserVViewModel : GitHubBaseViewModel
    {
        #region ブログマネージャー
        /// <summary>
        /// ブログマネージャー
        /// </summary>
        GitHubBlogUserManagerM _BlogManager = new GitHubBlogUserManagerM();
        /// <summary>
        /// ブログマネージャー
        /// </summary>
        public GitHubBlogUserManagerM BlogManager
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
        public ucGitHubUserVViewModel(IGlobalConfigM gConfig) : base(gConfig)
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
