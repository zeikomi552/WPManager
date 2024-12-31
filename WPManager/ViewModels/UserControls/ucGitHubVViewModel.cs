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

namespace WPManager.ViewModels.UserControls
{
    public class ucGitHubVViewModel : BindableBase
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
        GitHubDataObjectM _DataObject = new GitHubDataObjectM();
        /// <summary>
        /// データオブジェクト
        /// </summary>
        public GitHubDataObjectM DataObject
        {
            get
            {
                return _DataObject;
            }
            set
            {
                if (_DataObject == null || !_DataObject.Equals(value))
                {
                    _DataObject = value;
                    RaisePropertyChanged("DataObject");
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

        #region 記事
        /// <summary>
        /// 記事
        /// </summary>
        string _Article = string.Empty;
        /// <summary>
        /// 記事
        /// </summary>
        public string Article
        {
            get
            {
                return _Article;
            }
            set
            {
                if (_Article == null || !_Article.Equals(value))
                {
                    _Article = value;
                    RaisePropertyChanged("Article");
                }
            }
        }
        #endregion



        public ucGitHubVViewModel(IGlobalConfigM gConfig)
        {
            this.GitHubParameter = gConfig.GitHubConfig!;
        }


        public void Search()
        {
            Search(0);
        }
        #region 検索処理
        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="page">検索するページ</param>
        private async void Search(int page)
        {
            // GitHub Clientの作成
            var client = new GitHubClient(new ProductHeaderValue(this.GitHubParameter.ProductName));

            // トークンの取得
            var tokenAuth = new Credentials(this.GitHubParameter.AccessToken);
            client.Credentials = tokenAuth;

            SearchRepositoriesRequest request = new SearchRepositoriesRequest();
#pragma warning disable CS0618 // 型またはメンバーが旧型式です

            // 値を持っているかどうかのチェック
            request.Created = new DateRange(this.DataObject.SearchFrom, this.DataObject.SearchTo);

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
                this.Article = GetArticle(this.DataObject.SearchFrom, this.DataObject.SearchTo, this.SelectedLanguage, this.SearchResults!);
            }
        }
        #endregion

        #region 言語選択の解除処理
        /// <summary>
        /// 言語選択の解除処理
        /// </summary>
        public void LanguageClear()
        {
            this.SelectedLanguage = null;
        }
        #endregion

        /// <summary>
        /// 記事
        /// </summary>
        /// <returns>記事</returns>
        public static string GetArticle(DateTime startDt, DateTime endDt, Language? search_language, SearchRepositoryResult repogitories)
        {
            StringBuilder text = new StringBuilder();
            text.AppendLine($"## GitHubサーベイ 調査日{DateTime.Today.ToString("yyyy/MM/dd")}");

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

            return mdContents;
        }
    }
}
