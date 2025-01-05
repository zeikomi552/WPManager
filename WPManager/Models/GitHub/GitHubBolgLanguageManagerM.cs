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
using WPManager.Models.Civitai.Enums;
using WPManager.Models.GitHub.Enums;
using WPManager.Models.Schedule;

namespace WPManager.Models.GitHub
{
    public class GitHubBolgLanguageManagerM : GitHubBlogManagerBaseM
    {
        #region ブログタイプ
        /// <summary>
        /// ブログタイプ
        /// </summary>
        GitHubArticleType _ArticleType = GitHubArticleType.Type1;
        /// <summary>
        /// ブログタイプ
        /// </summary>
        public GitHubArticleType ArticleType
        {
            get
            {
                return _ArticleType;
            }
            set
            {
                if (!_ArticleType.Equals(value))
                {
                    _ArticleType = value;
                    RaisePropertyChanged("ArticleType");
                }
            }
        }
        #endregion

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

        #region 記事の作成処理
        /// <summary>
        /// 記事の作成処理
        /// </summary>
        /// <returns>記事</returns>
        protected override string GetArticle()
        {
            switch (this.ArticleType)
            {
                case GitHubArticleType.Type1:
                default:
                    {
                        return GetArticleType1();
                    }
                case GitHubArticleType.Type2:
                    {
                        return GetArticleType2();
                    }
            }
        }
        #endregion
        #region 記事作成処理
        /// <summary>
        /// 記事作成処理
        /// </summary>
        /// <returns>記事</returns>
        protected string GetArticleType1()
        {
            DateTime startDt = this.SearchCondition.SearchFrom;
            DateTime endDt = this.SearchCondition.SearchTo;

            StringBuilder text = new StringBuilder();
            text.AppendLine($"## GitHub調査日時 {DateTime.Now.ToString("yyyy/MM/dd(ddd) HH:mm:ss")}");

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

        public string GetArticleType2()
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

            var summary = (from x in this.SearchResults
                           group x by x.Language into g
                           select new
                           {
                               Language = g.Key,
                               StargazersCount = g.Sum(x => x.StargazersCount)
                           }).OrderByDescending(x => x.StargazersCount);

            int max = summary.Count();
            for (int i = 0; i < max; i ++)
            {
                var item = summary.ElementAt(i);
                string text = BlogListItem(i+1, item.Language, item.StargazersCount);
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

        private string BlogListItem(int rank, string language, int starCnt)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!-- wp:group {\"style\":{\"spacing\":{\"padding\":{\"top\":\"var:preset|spacing|40\",\"bottom\":\"var:preset|spacing|40\"}},\"border\":{\"top\":{\"color\":\"var:preset|color|accent-6\",\"width\":\"1px\"}}},\"layout\":{\"type\":\"flex\",\"flexWrap\":\"wrap\",\"justifyContent\":\"space-between\"}} -->");
            sb.AppendLine("<div class=\"wp-block-group\" style=\"border-top-color:var(--wp--preset--color--accent-6);border-top-width:1px;padding-top:var(--wp--preset--spacing--40);padding-bottom:var(--wp--preset--spacing--40)\">");
            sb.AppendLine("<!-- wp:group {\"layout\":{\"type\":\"constrained\"}} -->");
            sb.AppendLine("<div class=\"wp-block-group\"><!-- wp:heading {\"level\":3} -->");

            string lang = string.IsNullOrEmpty(language) ? "不明" : language;

            sb.AppendLine($"<h3 class=\"wp-block-heading\">{lang}</h3>");
            sb.AppendLine("<!-- /wp:heading -->");
            sb.AppendLine("");
            sb.AppendLine("<!-- wp:paragraph -->");
            sb.AppendLine($"<p>スター獲得数 : {starCnt}</p>");
            sb.AppendLine("<!-- /wp:paragraph --></div>");
            sb.AppendLine("<!-- /wp:group -->");
            sb.AppendLine("");
            sb.AppendLine("<!-- wp:group {\"style\":{\"spacing\":{\"blockGap\":\"var:preset|spacing|70\"}},\"layout\":{\"type\":\"flex\",\"flexWrap\":\"wrap\",\"justifyContent\":\"space-between\"}} -->");
            sb.AppendLine("<div class=\"wp-block-group\"><!-- wp:paragraph {\"style\":{\"typography\":{\"textTransform\":\"uppercase\"}}} -->");
            sb.AppendLine($"<p style=\"text-transform:uppercase\">{rank}位</p>");
            sb.AppendLine("<!-- /wp:paragraph -->");
            sb.AppendLine("");
            sb.AppendLine("<!-- wp:buttons -->");
            sb.AppendLine("<div class=\"wp-block-buttons\"><!-- wp:button {\"fontSize\":\"small\"} -->");
            sb.AppendLine("<div class=\"wp-block-button has-custom-font-size has-small-font-size\"><a class=\"wp-block-button__link wp-element-button\" " +
                $"href=\"https://www.google.co.jp/search?q={lang}\">" +
                "Google検索</a></div>");
            sb.AppendLine("<!-- /wp:button --></div>");
            sb.AppendLine("<!-- /wp:buttons --></div>");
            sb.AppendLine("<!-- /wp:group --></div>");
            sb.AppendLine("<!-- /wp:group -->");
            return sb.ToString();
        }

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

                GitHubBolgLanguageManagerM item = new GitHubBolgLanguageManagerM();

                // 基本検索条件をセット
                item.GitHubParameter.ProductName = config.GitHubConfig.ProductName;
                item.GitHubParameter.AccessToken = config.GitHubConfig.AccessToken;
                item.SearchCondition.SearchFrom = schdule_item.SearchFrom;
                item.SearchCondition.SearchTo = schdule_item.SearchTo;

                // 投稿記事 or 固定ページを設定 
                item.PostOrPage = schdule_item.PostPageType;

                // 記事Idをセット(新規作成の場合は0)
                item.Article.PostId = schdule_item.ArticleId;

                // 記事タイプ 1:簡素バージョン 2:豪華バージョン
                //item. = schdule_item.ArticleType == 1 ? CivitaiArticleType.Type1 : CivitaiArticleType.Type2;

                // 検索の実行
                var tmp = await item.SearchMaxSync(10);

                if (tmp)
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
