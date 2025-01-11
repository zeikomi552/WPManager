using Octokit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPManager.Models.GitHub
{
    public class GitHubBlogUserManagerM : GitHubBlogManagerBaseM
    {
        #region 検索処理
        /// <summary>
        /// 検索処理
        /// </summary>
        public override async void Search()
        {
            var result = await Search(1);   // 1ページ目の検索処理

            //var tmp = new ObservableCollection<Repository>(result);

            //// nullチェック
            //if (this.SearchResults != null)
            //{
            //    // 記事に関する各要素をセット
            //    SetArticleInfo();
            //}
        }
        #endregion


        #region 検索処理
        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="page">検索するページ</param>
        protected new  async Task<SearchRepositoryResult> Search(int page)
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
    }
}
