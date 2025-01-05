using Markdig;
using Octokit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPManager.Common.Extensions;

namespace WPManager.Models.GitHub
{
    public class GitHubBlogManagerBaseM : BaseBlogManagerM
    {
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

        #region 検索条件
        /// <summary>
        /// 検索条件
        /// </summary>
        GitHubDataObjectM _SearchCondition = new GitHubDataObjectM();
        /// <summary>
        /// 検索条件
        /// </summary>
        public GitHubDataObjectM SearchCondition
        {
            get
            {
                return _SearchCondition;
            }
            set
            {
                if (_SearchCondition == null || !_SearchCondition.Equals(value))
                {
                    _SearchCondition = value;
                    RaisePropertyChanged("SearchCondition");
                }
            }
        }
        #endregion

        #region 検索結果をリポジトリ形式で保管するリスト
        /// <summary>
        /// 検索結果をリポジトリ形式で保管するリスト
        /// </summary>
        ObservableCollection<Repository> _SearchResults = new ObservableCollection<Repository>();
        /// <summary>
        /// 検索結果をリポジトリ形式で保管するリスト
        /// </summary>
        public ObservableCollection<Repository> SearchResults
        {
            get
            {
                return _SearchResults;
            }
            set
            {
                if (_SearchResults == null || !_SearchResults.Equals(value))
                {
                    _SearchResults = value;
                    RaisePropertyChanged("SearchResults");
                }
            }
        }
        #endregion

        #region 指定したページ数まで検索を実行する
        /// <summary>
        /// 指定したページ数まで検索を実行する
        /// 1から始まる
        /// </summary>
        /// <param name="pagemax">ページ最大値</param>
        /// <returns>リポジトリデータ</returns>
        protected async Task<List<Repository>> SearchPageMax(int pagemax)
        {
            List<Repository> list = new List<Repository>();
            for (int page = 1; page <= pagemax; page++)
            {
                var result = await Search(page);

                list.AddRange(result.Items.ToList<Repository>());
            }

            return list;
        }
        #endregion

        #region 検索の実行処理
        /// <summary>
        /// 検索の実行処理
        /// </summary>
        public virtual async Task<bool> SearchSync()
        {
            try
            {
                var result = await Search(0);

                this.SearchResults = new ObservableCollection<Repository>(result.Items.ToList<Repository>());

                // nullチェック
                if (this.SearchResults != null)
                {
                    // 記事に関する各要素をセット
                    SetArticleInfo();
                }
                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

        }
        #endregion

        #region 検索の実行処理
        /// <summary>
        /// 検索の実行処理
        /// </summary>
        public virtual async void Search()
        {
            var result = await Search(0);

            this.SearchResults = new ObservableCollection<Repository>(result.Items.ToList<Repository>());

            // nullチェック
            if (this.SearchResults != null)
            {
                // 記事に関する各要素をセット
                SetArticleInfo();
            }
        }
        #endregion

        #region 検索処理
        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="page">検索するページ</param>
        protected async Task<SearchRepositoryResult> Search(int page)
        {
            // GitHub Clientの作成
            var client = new GitHubClient(new ProductHeaderValue(this.GitHubParameter.ProductName));

            // トークンの取得
            var tokenAuth = new Credentials(this.GitHubParameter.AccessToken);
            client.Credentials = tokenAuth;

            SearchRepositoriesRequest request = new SearchRepositoriesRequest();
#pragma warning disable CS0618 // 型またはメンバーが旧型式です

            // 値を持っているかどうかのチェック
            request.Created = new DateRange(this.SearchCondition.SearchFrom, this.SearchCondition.SearchTo);

            // スターの数
            request.Stars = new Octokit.Range(1, int.MaxValue);

            // 読み込むページ
            request.Page = page;

            // スターの数でソート
            request.SortField = RepoSearchSort.Stars;

            if (this.SearchCondition.SelectedLanguage.HasValue)
            {
                request.Language = this.SearchCondition.SelectedLanguage;
            }

            // 降順でソート
            request.Order = SortDirection.Descending;
#pragma warning restore CS0618 // 型またはメンバーが旧型式です

            return await client.Search.SearchRepo(request);

        }
        #endregion

        #region ブログの記事情報をセットする
        /// <summary>
        /// ブログの記事情報をセットする
        /// </summary>
        protected virtual void SetArticleInfo()
        {
            this.Article.Title = GetTitle();
            this.Article.Slug = GetSlug();
            this.Article.Content = GetArticle();
            this.Article.Description = GetDescription();
            this.Article.Excerpt = GetExcerpt();
            RaisePropertyChanged("Article");
        }
        #endregion

        #region 記事作成処理
        /// <summary>
        /// 記事作成処理
        /// </summary>
        /// <returns>記事</returns>
        protected virtual string GetArticle()
        {
            return string.Empty;
        }
        #endregion

        #region スラッグを作成する
        /// <summary>
        /// スラッグを作成する
        /// </summary>
        /// <returns>スラッグ</returns>
        protected virtual string GetSlug()
        {
            return string.Empty;
        }
        #endregion

        #region タイトルの作成処理
        /// <summary>
        /// タイトルの作成処理
        /// </summary>
        /// <returns>タイトル</returns>
        protected virtual string GetTitle()
        {
            return string.Empty;

        }
        #endregion

        #region 詳細の作成処理
        /// <summary>
        /// 詳細の作成処理
        /// </summary>
        /// <returns>詳細</returns>
        protected virtual string GetDescription()
        {
            return string.Empty;
        }
        #endregion

        #region 要約の作成処理
        /// <summary>
        /// 要約の作成処理
        /// </summary>
        /// <returns>要約</returns>
        protected virtual string GetExcerpt()
        {
            return string.Empty;

        }
        #endregion
    }
}
