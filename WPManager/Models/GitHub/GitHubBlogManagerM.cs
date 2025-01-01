using Markdig;
using Octokit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPManager.Common.Extensions;
using WPManager.Models.Civitai.Enums;

namespace WPManager.Models.GitHub
{
    /// <summary>
    /// GitHubのリポジトリ検索ブログ管理クラス
    /// </summary>
    public class GitHubBlogManagerM : GitHubBlogManagerBaseM
    {
        #region 検索処理
        /// <summary>
        /// 検索処理
        /// </summary>
        public override void Search()
        {
            base.Search();
        }
        #endregion

        #region ブログの記事情報をセットする
        /// <summary>
        /// ブログの記事情報をセットする
        /// </summary>
        protected override void SetArticleInfo()
        {
            this.Article.Title = GetTitle();
            this.Article.Slug = GetSlug();
            this.Article.Content = GetArticle();
            this.Article.Description = GetTitle();
            this.Article.Excerpt = GetTitle();
            RaisePropertyChanged("Article");
        }
        #endregion

        #region 記事作成処理
        /// <summary>
        /// 記事作成処理
        /// </summary>
        /// <returns>記事</returns>
        protected override string GetArticle()
        {
            DateTime startDt = this.SearchCondition.SearchFrom;
            DateTime endDt = this.SearchCondition.SearchTo;

            StringBuilder text = new StringBuilder();
            text.AppendLine($"## GitHub調査日{DateTime.Today.ToString("yyyy/MM/dd")}");

            text.AppendLine($"### 検索条件");

            text.AppendLine($"- リポジトリ作成日 {startDt.ToString("yyyy/MM/dd")} - {endDt.ToString("yyyy/MM/dd")}");

            // 言語条件
            if (this.SearchCondition.SelectedLanguage.HasValue)
            {
                text.AppendLine($"- 開発言語 {this.SearchCondition.SelectedLanguage}");
            }

            text.AppendLine($"- ソート順：スター獲得数順");
            text.AppendLine();

            text.AppendLine($"### 検索結果");
            text.AppendLine($"|スター(順位)<br>使用言語|リポジトリ名<br>説明|検索|");
            text.AppendLine($"|:-:|---|:-:|");
            int rank = 1;
            foreach (var repo in this.SearchResults)
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

        #region スラッグを作成する
        /// <summary>
        /// スラッグを作成する
        /// </summary>
        /// <returns>スラッグ</returns>
        protected override string GetSlug()
        {
            string period = this.SearchCondition.SearchFrom.ToString("yyyyMMdd") + "-" + this.SearchCondition.SearchTo.ToString("yyyyMMdd");
            string lang = this.SearchCondition.SelectedLanguage.HasValue ? this.SearchCondition.SelectedLanguage.Value.ToString() : "all";

            return ($"{period}-githab-{lang}");
        }
        #endregion

        #region タイトルの作成処理
        /// <summary>
        /// タイトルの作成処理
        /// </summary>
        /// <returns>タイトル</returns>
        protected override string GetTitle()
        {
            string title = string.Empty;
            string period = this.SearchCondition.SearchFrom.ToString("yyyy/MM/dd") + "-" + this.SearchCondition.SearchTo.ToString("yyyy/MM/dd");
            string lang = this.SearchCondition.SelectedLanguage.HasValue ? this.SearchCondition.SelectedLanguage.Value.ToString() : "";
            string text = "年版 GitHub人気リポジトリ速報！スター獲得数ランキング";

            if (this.SearchCondition.SelectedLanguage.HasValue)
            {
                switch (this.SearchCondition.SelectedLanguage)
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
                            title = ($"{this.SearchCondition.SearchFrom.Year}{text}({this.SearchCondition.SelectedLanguage.Value})");
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


        #region 詳細の作成処理
        /// <summary>
        /// 詳細の作成処理
        /// </summary>
        /// <returns>詳細</returns>
        protected override string GetDescription()
        {
            return GetTitle();
        }
        #endregion

        #region 要約の作成処理
        /// <summary>
        /// 要約の作成処理
        /// </summary>
        /// <returns>要約</returns>
        protected override string GetExcerpt()
        {
            return GetTitle();

        }
        #endregion
    }
}
