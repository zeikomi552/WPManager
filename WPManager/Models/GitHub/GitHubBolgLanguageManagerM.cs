using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPManager.Models.GitHub
{
    public class GitHubBolgLanguageManagerM : BaseBlogManagerM
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

        #region データオブジェクト
        /// <summary>
        /// データオブジェクト
        /// </summary>
        GitHubDataObjectM _SearchCondition = new GitHubDataObjectM();
        /// <summary>
        /// データオブジェクト
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

        List<Repository> _ResultList = new List<Repository>();



        #region 検索処理
        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="page">検索するページ</param>
        public async Task<string> SearchLanguage(int max)
        {
            for (int i = 1; i <= max; i++)
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
                request.Page = i;

                // スターの数でソート
                request.SortField = RepoSearchSort.Stars;

                if (this.SearchCondition.SelectedLanguage.HasValue)
                {
                    request.Language = this.SearchCondition.SelectedLanguage;
                }

                // 降順でソート
                request.Order = SortDirection.Descending;
#pragma warning restore CS0618 // 型またはメンバーが旧型式です

                var tmp = await client.Search.SearchRepo(request);
                this._ResultList.AddRange(tmp.Items);
            }

            var summary = (from x in this._ResultList
                           group x by x.Language into g
                           select new
                           {
                               Language = g.Key,
                               StargazersCount = g.Sum(x => x.StargazersCount)
                           }).OrderByDescending(x => x.StargazersCount);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("|言語|スター獲得数|");
            sb.AppendLine("|:-:|:-:|");
            foreach (var tmp in summary)
            {
                string lang = string.IsNullOrEmpty(tmp.Language) ? "不明" : tmp.Language;
                sb.AppendLine($"|{lang}|{tmp.StargazersCount}|");
            }

            return sb.ToString();
        }
        #endregion
    }
}
