using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPManager.Models;
using WPManager.Models.GitHub;

namespace WPManager.ViewModels.UserControls
{
    public class ucGitHubLanguageVViewModel : GitHubBaseViewModel
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

        public ucGitHubLanguageVViewModel(IGlobalConfigM gConfig) : base(gConfig)
        {
            this.BlogManager.GitHubParameter = gConfig.GitHubConfig!;
        }


        #region 検索処理
        /// <summary>
        /// 検索処理
        /// </summary>
        public async void Search()
        {
            this.BlogManager.Article.Content = await this.BlogManager.SearchLanguage(10);
            RaisePropertyChanged("BlogManager");
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
