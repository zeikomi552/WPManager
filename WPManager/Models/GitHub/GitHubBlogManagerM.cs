using ControlzEx.Standard;
using Markdig;
using Octokit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPManager.Common.Extensions;
using WPManager.Models.Civitai;
using WPManager.Models.Civitai.Enums;
using WPManager.Models.GitHub.Enums;
using WPManager.Models.Schedule;

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


        #region 検索処理
        /// <summary>
        /// 検索処理
        /// </summary>
        public override async Task<bool> SearchSync(int page = 0)
        {
            try
            {
                return await base.SearchSync(page);
            }
            catch
            {
                return false;
            }
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
        protected override string GetArticleType1()
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

        #region ブログの検索条件文字列作成処理
        /// <summary>
        /// ブログの検索条件文字列作成処理
        /// </summary>
        /// <returns>ブログの検索条件文字列</returns>
        private string BlogSearchCondition()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<!-- wp:paragraph --> ");
            DateTime startDt = this.SearchCondition.SearchFrom;
            DateTime endDt = this.SearchCondition.SearchTo;
            sb.Append($"<p>調査対象 {startDt.ToString("yyyy/MM/dd")} - {endDt.ToString("yyyy/MM/dd")}, リポジトリ数 {this.SearchResults.Count()}, スター獲得数順</p> ");
            sb.Append("<!-- /wp:paragraph --> ");
            return sb.ToString();

        }
        #endregion


        private string BlogListItem(int rank, Repository repos)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!-- wp:group {\"style\":{\"spacing\":{\"padding\":{\"top\":\"var:preset|spacing|40\",\"bottom\":\"var:preset|spacing|40\"}},\"border\":{\"top\":{\"color\":\"var:preset|color|accent-6\",\"width\":\"1px\"}}},\"layout\":{\"type\":\"flex\",\"flexWrap\":\"wrap\",\"justifyContent\":\"space-between\"}} -->");
            sb.AppendLine("<div class=\"wp-block-group\" style=\"border-top-color:var(--wp--preset--color--accent-6);border-top-width:1px;padding-top:var(--wp--preset--spacing--40);padding-bottom:var(--wp--preset--spacing--40)\">");
            sb.AppendLine("<!-- wp:group {\"layout\":{\"type\":\"constrained\"}} -->");
            sb.AppendLine("<div class=\"wp-block-group\"><!-- wp:heading {\"level\":3} -->");

            string repoName = string.IsNullOrEmpty(repos.FullName) ? "不明" : repos.FullName;

            sb.AppendLine($"<h3 class=\"wp-block-heading is-style-text-subtitle\">{rank}位  {repoName}</h3>");
            sb.AppendLine("<!-- /wp:heading -->");
            sb.AppendLine("");
            sb.AppendLine("<!-- wp:paragraph -->");
            sb.AppendLine($"Star : {repos.StargazersCount}<br>");
            sb.AppendLine($"Language : {repos.Language}<br>");
            sb.AppendLine($"Owner : {repos.Owner.Login}<br>");
            sb.AppendLine($"Description : {repos.Description.EmptyToText("-").CutText(50).Replace("|", "\\/")}<br>");
            sb.AppendLine("<!-- /wp:paragraph --></div>");
            sb.AppendLine("<!-- /wp:group -->");
            sb.AppendLine("");
            sb.AppendLine("<!-- wp:group {\"style\":{\"spacing\":{\"blockGap\":\"var:preset|spacing|70\"}},\"layout\":{\"type\":\"flex\",\"flexWrap\":\"wrap\",\"justifyContent\":\"space-between\"}} -->");
            sb.AppendLine("<div class=\"wp-block-group\">"); 
            sb.AppendLine("");
            sb.AppendLine("<!-- wp:buttons -->");
            sb.AppendLine("<div class=\"wp-block-buttons\">");

            if (!string.IsNullOrEmpty(repos.HtmlUrl))
            {
                sb.AppendLine("<!-- wp:button {\"fontSize\":\"small\"} -->");
                sb.AppendLine("<div class=\"wp-block-button has-custom-font-size has-small-font-size\"><a class=\"wp-block-button__link wp-element-button\" " +
                    $"href=\"{repos.HtmlUrl}\">" +
                    "GitHub</a></div>");
                sb.AppendLine("<!-- /wp:button -->");
            }
            if (!string.IsNullOrEmpty(repos.Owner.HtmlUrl))
            {
                sb.AppendLine("<!-- wp:button {\"fontSize\":\"small\"} -->");
                sb.AppendLine("<div class=\"wp-block-button has-custom-font-size has-small-font-size\"><a class=\"wp-block-button__link wp-element-button\" " +
                    $"href=\"{repos.Owner.HtmlUrl}\">" +
                    "Owner Page</a></div>");
                sb.AppendLine("<!-- /wp:button -->");
            }

            if (!string.IsNullOrEmpty(repos.Homepage) && !repos.Homepage.Equals("https://github.com"))
            {
                sb.AppendLine("<!-- wp:button {\"fontSize\":\"small\"} -->");
                sb.AppendLine("<div class=\"wp-block-button has-custom-font-size has-small-font-size\"><a class=\"wp-block-button__link wp-element-button\" " +
                    $"href=\"{repos.Homepage}\">" +
                    "Homepage</a></div>");
                sb.AppendLine("<!-- /wp:button -->");
            }
            if (!string.IsNullOrEmpty(repos.Owner.Blog))
            {
                sb.AppendLine("<!-- wp:button {\"fontSize\":\"small\"} -->");
                sb.AppendLine("<div class=\"wp-block-button has-custom-font-size has-small-font-size\"><a class=\"wp-block-button__link wp-element-button\" " +
                    $"href=\"{repos.Owner.Blog}\">" +
                    "Homepage</a></div>");
                sb.AppendLine("<!-- /wp:button -->");
            }


            sb.AppendLine("</div>");
            sb.AppendLine("<!-- /wp:buttons --></div>");
            sb.AppendLine("<!-- /wp:group --></div>");
            sb.AppendLine("<!-- /wp:group -->");
            return sb.ToString();
        }

        #region 記事作成処理(デザイン性のあるレイアウト)
        /// <summary>
        /// 記事作成処理(デザイン性のあるレイアウト)
        /// </summary>
        /// <returns></returns>
        protected override string GetArticleType2()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!-- wp:group {\"metadata\":{\"categories\":[\"twentytwentyfive_page\"],\"patternName\":\"core/block/421\",\"name\":\"WPManagerテンプレート\"},\"align\":\"full\",\"style\":{\"spacing\":{\"padding\":{\"top\":\"var:preset|spacing|50\",\"bottom\":\"var:preset|spacing|50\"},\"margin\":{\"top\":\"0\",\"bottom\":\"0\"}}},\"layout\":{\"type\":\"constrained\"}} -->");
            sb.AppendLine("<div class=\"wp-block-group alignfull\" style=\"margin-top:0;margin-bottom:0;padding-top:var(--wp--preset--spacing--50);padding-bottom:var(--wp--preset--spacing--50)\">");
            sb.AppendLine("<!-- wp:group {\"align\":\"wide\",\"layout\":{\"type\":\"default\"}} -->");
            sb.AppendLine("<div class=\"wp-block-group alignwide\"><!-- wp:heading -->");
            sb.AppendLine("<h2 class=\"wp-block-heading\">GitHub人気言語ランキング</h2>");
            sb.AppendLine("<!-- /wp:heading -->");
            sb.AppendLine("");
            sb.AppendLine(BlogSearchCondition());
            sb.AppendLine("");
            sb.AppendLine("<!-- wp:group {\"style\":{\"spacing\":{\"blockGap\":\"0\",\"margin\":{\"top\":\"var:preset|spacing|70\"}}},\"layout\":{\"type\":\"default\"}} -->");
            sb.AppendLine("<div class=\"wp-block-group\" style=\"margin-top:var(--wp--preset--spacing--70)\">");

            int max = this.SearchResults.Count();
            for (int i = 0; i < max; i++)
            {
                var item = this.SearchResults.ElementAt(i);
                string text = BlogListItem(i + 1, item);
                sb.AppendLine(text);

                if (i + 1 == max)
                {
                    sb.AppendLine("</div>");
                }
            }

            sb.AppendLine("<!-- /wp:group --></div>");
            sb.AppendLine("<!-- /wp:group --></div>");
            sb.AppendLine("<!-- /wp:group -->");

            return sb.ToString();

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


        #region 検索と投稿を実行する
        /// <summary>
        /// 検索と投稿を実行する
        /// </summary>
        /// <param name="schdule_item">スケジュール設定データ</param>
        /// <param name="wpPrame">WPコンフィグ情報</param>
        public async void SearchAndPost(ScheduleM schdule_item, IGlobalConfigM config)
        {
            try
            {
                if (config == null || config.GitHubConfig == null || config.WPConfig == null)
                    return;

                GitHubBlogManagerM item = new GitHubBlogManagerM();

                // 基本検索条件をセット
                item.GitHubParameter.ProductName = config.GitHubConfig.ProductName;
                item.GitHubParameter.AccessToken = config.GitHubConfig.AccessToken;
                item.SearchCondition.SearchFrom = schdule_item.SearchFrom;
                item.SearchCondition.SearchTo = schdule_item.SearchTo;
                item.SearchCondition.SelectedLanguage = schdule_item.GitHubLanguage;
                //item.GitHubParameter = wpPrame;

                // 投稿記事 or 固定ページを設定 
                item.PostOrPage = schdule_item.PostPageType;

                // 記事Idをセット(新規作成の場合は0)
                item.Article.PostId = schdule_item.ArticleId;

                // 記事タイプ 1:簡素バージョン 2:豪華バージョン
                item.ArticleType = schdule_item.ArticleType == 1 ? GitHubArticleType.Type1 : GitHubArticleType.Type2;

                // 検索の実行
                bool ret = await item.SearchSync();

                if (ret)
                {
                    // 記事タイトル
                    item.Article.Title = schdule_item.Title;

                    // ポストの実行
                    item.Post(config.WPConfig);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
    }
}
