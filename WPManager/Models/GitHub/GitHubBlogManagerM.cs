using Markdig;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPManager.Common.Extensions;
using WPManager.Models.Civitai.Enums;

namespace WPManager.Models.GitHub
{
    public class GitHubBlogManagerM : BaseBlogManagerM
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

        #region 検索結果
        /// <summary>
        /// 検索結果
        /// </summary>
        SearchRepositoryResult? _SearchResults = new SearchRepositoryResult();
        /// <summary>
        /// 検索結果
        /// </summary>
        public SearchRepositoryResult? SearchResults
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

        #region 検索処理
        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="page">検索するページ</param>
        public async void Search(int page)
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

            if (this.SelectedLanguage.HasValue)
            {
                request.Language = this.SelectedLanguage;
            }

            // 降順でソート
            request.Order = SortDirection.Descending;
#pragma warning restore CS0618 // 型またはメンバーが旧型式です

            this.SearchResults = await client.Search.SearchRepo(request);

            // nullチェック
            if (this.SearchResults != null)
            {
                // 記事に関する各要素をセット
                SetArticleInfo();
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

                if (this.SelectedLanguage.HasValue)
                {
                    request.Language = this.SelectedLanguage;
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

        public void SearchLoop(int loopcount)
        {
            for (int i = 0; i < loopcount; i++)
            {
                Search(i);
            }
        }
        #region 記事作成処理
        /// <summary>
        /// 記事作成処理
        /// </summary>
        /// <returns>記事</returns>
        public static string GetArticle(DateTime startDt, DateTime endDt, Language? search_language, SearchRepositoryResult repogitories)
        {
            StringBuilder text = new StringBuilder();
            text.AppendLine($"## GitHub調査日{DateTime.Today.ToString("yyyy/MM/dd")}");

            text.AppendLine($"### 検索条件");

            text.AppendLine($"- リポジトリ作成日 {startDt.ToString("yyyy/MM/dd")} - {endDt.ToString("yyyy/MM/dd")}");

            // 言語条件
            if (search_language.HasValue)
            {
                text.AppendLine($"- 開発言語 {search_language}");
            }

            text.AppendLine($"- ソート順：スター獲得数順");
            text.AppendLine();

            text.AppendLine($"### 検索結果");
            text.AppendLine($"|スター(順位)<br>使用言語|リポジトリ名<br>説明|検索|");
            text.AppendLine($"|:-:|---|:-:|");
            int rank = 1;
            foreach (var repo in repogitories.Items)
            {
                string description = repo.Description.EmptyToText("-").CutText(50).Replace("|", "\\/");
                string language = repo.Language.EmptyToText("-").CutText(20);

                string homepage_url = !string.IsNullOrWhiteSpace(repo.Homepage) ? $" [[Home Page]({repo.Homepage})]" : string.Empty;

                // 行情報の作成
                text.AppendLine($"|<center>{repo.StargazersCount}<br>({rank++}位)<br>{language}</center>|" +
                    $"[{repo.FullName}]({repo.HtmlUrl}){homepage_url}<br>{description}|" +
                    $"[[google](https://www.google.com/search?q={repo.Name})] " +
                    $"[[Qiita](https://qiita.com/search?q={repo.Name})]|");
            }

            //convert Mark down to html and set to mdContents
            Markdig.MarkdownPipeline markdownPipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();

            string mdContents = Markdown.ToHtml(text.ToString(), markdownPipeline);

            return mdContents.Replace("<table>", "<table border=\"1\">");
        }
        #endregion

        #region ブログの記事情報をセットする
        /// <summary>
        /// ブログの記事情報をセットする
        /// </summary>
        private void SetArticleInfo()
        {
            this.Article.Title = CreateTitle();
            this.Article.Slug = CreateSlug();
            this.Article.Content = CreateArticle();
            this.Article.Description = CreateTitle();
            this.Article.Excerpt = CreateTitle();
            RaisePropertyChanged("Article");
        }
        #endregion

        public string CreateArticle()
        {
            return GetArticle(this.SearchCondition.SearchFrom,
                this.SearchCondition.SearchTo, this.SelectedLanguage, this.SearchResults!);
        }

        #region スラッグを作成する
        /// <summary>
        /// スラッグを作成する
        /// </summary>
        /// <returns>スラッグ</returns>
        private string CreateSlug()
        {
            string period = this.SearchCondition.SearchFrom.ToString("yyyyMMdd") + "-" + this.SearchCondition.SearchTo.ToString("yyyyMMdd");
            string lang = this.SelectedLanguage.HasValue ? this.SelectedLanguage.Value.ToString() : "all";

            return ($"{period}-githab-{lang}");
        }
        #endregion


        #region タイトルの作成処理
        /// <summary>
        /// タイトルの作成処理
        /// </summary>
        /// <returns>タイトル</returns>
        private string CreateTitle()
        {
            string title = string.Empty;
            string period = this.SearchCondition.SearchFrom.ToString("yyyy/MM/dd") + "-" + this.SearchCondition.SearchTo.ToString("yyyy/MM/dd");
            string lang = this.SelectedLanguage.HasValue ? this.SelectedLanguage.Value.ToString() : "";
            string text = "年版 GitHub人気リポジトリ速報！スター獲得数ランキング";

            if (this.SelectedLanguage.HasValue)
            {
                switch (this.SelectedLanguage)
                {
                    case Language.CSharp:
                        {
                            title = ($"{this.SearchCondition.SearchFrom.Year}{text}(C#)");
                            break;
                        }
                    case Language.CPlusPlus:
                        {
                            title = ($"{this.SearchCondition.SearchFrom.Year}{text}(C++)");
                            break;
                        }
                    default:
                        {
                            title = ($"{this.SearchCondition.SearchFrom.Year}{text}({this.SelectedLanguage.Value})");
                            break;
                        }
                }
            }
            else
            {
                title = ($"{this.SearchCondition.SearchFrom.Year}{text}");
            }

            return title;
        }
        #endregion
    }
}
