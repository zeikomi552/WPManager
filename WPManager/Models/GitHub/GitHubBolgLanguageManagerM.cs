using Markdig;
using Octokit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPManager.Common.Extensions;

namespace WPManager.Models.GitHub
{
    public class GitHubBolgLanguageManagerM : GitHubBlogManagerBaseM
    {
        #region 検索処理
        /// <summary>
        /// 検索処理
        /// </summary>
        public override async void Search()
        {
            var result = await SearchPageMax(10);   // 1ページ目の検索処理

            this.SearchResults = new ObservableCollection<Repository>(result);

            // nullチェック
            if (this.SearchResults != null)
            {
                // 記事に関する各要素をセット
                SetArticleInfo();
            }
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
            text.AppendLine($"- 検索リポジトリ数 {this.SearchResults.Count}件");
            text.AppendLine($"- ソート順：スター獲得数順");
            text.AppendLine();

            var summary = (from x in this.SearchResults
                           group x by x.Language into g
                           select new
                           {
                               Language = g.Key,
                               StargazersCount = g.Sum(x => x.StargazersCount)
                           }).OrderByDescending(x => x.StargazersCount);

            text.AppendLine($"### 検索結果");
            text.AppendLine("|言語|スター獲得数|");
            text.AppendLine("|:-:|:-:|");
            foreach (var tmp in summary)
            {
                string lang = string.IsNullOrEmpty(tmp.Language) ? "不明" : tmp.Language + $" / [Wiki](https://ja.wikipedia.org/wiki/{tmp.Language.Replace(" ", "%20")})";
                text.AppendLine($"|{lang}|{tmp.StargazersCount}|");
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
            string period = this.SearchCondition.SearchFrom.ToString("yyyy");
            string lang = this.SearchCondition.SelectedLanguage.HasValue ? this.SearchCondition.SelectedLanguage.Value.ToString() : "all";

            return ($"{period}-language-ranking");
        }
        #endregion

        #region タイトルの作成処理
        /// <summary>
        /// タイトルの作成処理
        /// </summary>
        /// <returns>タイトル</returns>
        protected override string GetTitle()
        {
            return $"GitHubのプログラミング言語人気ランキング！ {this.SearchCondition.SearchFrom.ToString("yyyy")}年版";
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
