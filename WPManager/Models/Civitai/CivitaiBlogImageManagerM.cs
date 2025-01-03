using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPManager.Common.Extensions;
using WPManager.Models.Civitai.Enums;
using WPManager.Models.Civitai.Images;
using WPManager.Models.Civitai.Models;

namespace WPManager.Models.Civitai
{
    public class CivitaiBlogImageManagerM : BaseBlogManagerM
    {
        #region ブログタイプ
        /// <summary>
        /// ブログタイプ
        /// </summary>
        CivitaiArticleType _ArticleType = CivitaiArticleType.Type1;
        /// <summary>
        /// ブログタイプ
        /// </summary>
        public CivitaiArticleType ArticleType
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

        #region エンドポイント
        /// <summary>
        /// エンドポイント
        /// </summary>
        const string EndPoint = "https://civitai.com/api/v1/images";
        #endregion

        #region 検索条件
        /// <summary>
        /// 検索条件
        /// </summary>
        CvsImageSearchM _SearchCondition = new CvsImageSearchM();
        /// <summary>
        /// 検索条件
        /// </summary>
        public CvsImageSearchM SearchCondition
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
        CvsImageM _SearchResults = new CvsImageM();
        /// <summary>
        /// 検索結果
        /// </summary>
        public CvsImageM SearchResults
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


        #region 検索処理
        /// <summary>
        /// 検索処理
        /// </summary>
        public async void Search()
        {
            var client = new CivitaiClient(EndPoint);
            this.SearchResults = await client.ImageSearch(this.SearchCondition.RequestQuery);
            SetArticleInfo();
        }
        #endregion

        #region ブログの記事情報をセットする
        /// <summary>
        /// ブログの記事情報をセットする
        /// </summary>
        private void SetArticleInfo()
        {
            this.Article.Title = GetTitle();
            this.Article.Slug = GetSlug();
            this.Article.Content = GetArticle();
            this.Article.Description = GetDescription();
            this.Article.Excerpt = GetExcerpt();
            RaisePropertyChanged("Article");
        }
        #endregion

        #region スラッグを作成する
        /// <summary>
        /// スラッグを作成する
        /// </summary>
        /// <returns>スラッグ</returns>
        private string GetSlug()
        {
            string slug = string.Empty;

            string period = "all";
            if (this.SearchCondition.Period.HasValue)
            {
                switch (this.SearchCondition.Period)
                {
                    case ModelPeriodEnum.Year:
                        {
                            period = "yearly";
                            break;
                        }
                    case ModelPeriodEnum.Month:
                        {
                            period = "monthly";
                            break;
                        }
                    case ModelPeriodEnum.Week:
                        {
                            period = "weekly";
                            break;
                        }
                    case ModelPeriodEnum.Day:
                        {
                            period = "daily";
                            break;
                        }
                    case ModelPeriodEnum.AllTime:
                    case ModelPeriodEnum.Empty:
                        {
                            period = "allperiod";
                            break;
                        }
                }
            }

            slug = "civitai-image-" + period + "-" + this.ArticleType.ToString();
            return slug;
        }
        #endregion

        #region タイトルの取得
        /// <summary>
        /// タイトルの取得
        /// </summary>
        /// <returns>タイトルの取得</returns>
        private string GetTitle()
        {
            switch (this.ArticleType)
            {
                case CivitaiArticleType.Type1:
                    {
                        return GetTitleSub();
                    }
                case CivitaiArticleType.Type2:
                    {
                        return $"{GetPeriod()} Civitaiイメージランキング";
                    }
                default:
                    {
                        return GetTitleSub();
                    }
            }

        }
        #endregion

        #region 期間の取得
        /// <summary>
        /// 期間の取得
        /// </summary>
        /// <returns>期間文字列</returns>
        private string GetPeriod()
        {
            string period = "全期間";
            if (this.SearchCondition.Period.HasValue)
            {
                switch (this.SearchCondition.Period)
                {
                    case ModelPeriodEnum.Year:
                        {
                            period = "年間";
                            break;
                        }
                    case ModelPeriodEnum.Month:
                        {
                            period = "月間";
                            break;
                        }
                    case ModelPeriodEnum.Week:
                        {
                            period = "週間";
                            break;
                        }
                    case ModelPeriodEnum.Day:
                        {
                            period = "デイリー";
                            break;
                        }
                    case ModelPeriodEnum.AllTime:
                    case ModelPeriodEnum.Empty:
                        {
                            period = "全期間";
                            break;
                        }
                }
            }
            return period;
        }
        #endregion

        #region タイトルの作成処理
        /// <summary>
        /// タイトルの作成処理
        /// </summary>
        /// <returns>タイトル</returns>
        private string GetTitleSub()
        {
            string title = string.Empty;

            string period = GetPeriod();
           

            title = ($"{DateTime.Today.Year}年版 CIVITAI人気画像速報！{period}スター獲得数ランキング");

            if (this.ArticleType == CivitaiArticleType.Type2)
            {
                title = title + " 画像付き！";
            }

            return title;
        }
        #endregion

        #region 記事の作成処理
        /// <summary>
        /// 記事の作成処理
        /// </summary>
        /// <returns>記事</returns>
        private string GetArticle()
        {
            switch (this.ArticleType)
            {
                case CivitaiArticleType.Type1:
                default:
                    {
                        return GetArticleType1();
                    }
                case CivitaiArticleType.Type2:
                    {
                        return GetArticleType2();
                    }
            }
        }
        #endregion

        #region 記事タイプ1の取得(画像なしリスト)
        /// <summary>
        /// 記事タイプ1の取得(画像なしリスト)
        /// </summary>
        /// <returns>記事</returns>
        private string GetArticleType1()
        {
            //// ダイアログのインスタンスを生成
            //var dialog = new SaveFileDialog();

            //// ファイルの種類を設定
            //dialog.Filter = "マークダウン (*.md)|*.md";

            StringBuilder sb = new StringBuilder();

            int rank = 1;
            int imageid = 1;
            foreach (var item in this.SearchResults.Items)
            {
                sb.AppendLine($"<!-- wp:heading {{\"backgroundColor\":\"accent-5\"}} -->");
                sb.AppendLine($"<h2 class=\"wp-block-heading has-accent-5-background-color has-background\">{rank++}位 {item.Username}</h2>");
                sb.AppendLine($"<!-- /wp:heading -->");
                sb.AppendLine($"");

                sb.AppendLine($"<!-- wp:list -->");
                sb.AppendLine($"<ul>");
                sb.AppendLine($"<li>CreatedAt : {item.CreatedAt.ToString("yyyy/MM/dd")}</li>");
                sb.AppendLine($"<li>Author : <a href=\"https://civitai.com/user/{item.Username}\">{item.Username}</a></li>");
                sb.AppendLine($"<li>Nsfw : {item.Nsfw}</li>");
                sb.AppendLine($"<li>NsfwLevel : {item.NsfwLevel}</li>");
                sb.AppendLine($"<li>Count : Like({item.Stats.LikeCount}) Heart({item.Stats.HeartCount})</li>");
                sb.AppendLine($"<li><a href=\"{item.ImageURL}\">{item.ImageURL}</a></li>");
                sb.AppendLine($"</ul>");
                sb.AppendLine($"<!-- /wp:list -->");

                string ext = System.IO.Path.GetExtension(item.Url).ToLower();
                if (ext.Equals(".mp4"))
                {
                    sb.AppendLine("<!-- wp:group {\"layout\":{\"type\":\"constrained\",\"contentSize\":\"300px\"}} -->");
                    sb.AppendLine($"<div class=\"wp-block-group\">");
                    sb.AppendLine($"<!-- wp:video -->");
                    sb.AppendLine($"<figure class=\"wp-block-video\"><video controls src=\"{item.Url}\"></video></figure>");
                    sb.AppendLine($"<!-- /wp:video -->");
                    sb.AppendLine($"</div>");
                    sb.AppendLine($"<!-- /wp:group -->");
                }
                else
                {

                    sb.AppendLine($"<!-- wp:image {{\"width\":\"300px\",\"linkDestination\":\"none\",\"align\":\"center\",\"className\":\"size-large\"}} -->");
                    sb.AppendLine($"<figure class=\"wp-block-image aligncenter is-resized size-large\"><img src=\"{item.Url}\"  alt=\"\" style=\"width:300px\"/></figure>");
                    sb.AppendLine($"<!-- /wp:image -->");
                }

                sb.AppendLine($"");

                if (item.Meta != null)
                {
                    sb.AppendLine($"<h3>Model</h3>");

                    if (!string.IsNullOrEmpty(item.Meta.Model))
                    {
                        sb.AppendLine($"<ul>");
                        sb.AppendLine($"<li>");
                        sb.AppendLine($"<a href=\"https://www.google.co.jp/search?q=civitai/{item.Meta.Model}\">{item.Meta.Model}</a>");
                        sb.AppendLine($"</li>");
                        sb.AppendLine($"</ul>");
                    }
                    else
                    {
                        sb.AppendLine($"<ul>");
                        sb.AppendLine($"<li>記載なし</li>");
                        sb.AppendLine($"</ul>");
                    }

                    if (!string.IsNullOrEmpty(item.Meta.Prompt))
                    {
                        sb.AppendLine($"<!-- wp:loos-hcb/code-block {{\"langType\":\"prompt\",\"langName\":\"Prompt\"}} -->");
                        sb.AppendLine($"<div class=\"hcb_wrap\"><pre class=\"prism undefined-numbers lang-prompt\" data-lang=\"Prompt\">");
                        sb.AppendLine($"<code>{item.Meta.Prompt.HtmlCodeEscape()}</code>");
                        sb.AppendLine($"</pre></div>");
                        sb.AppendLine($"<!-- /wp:loos-hcb/code-block -->");
                    }
                    else
                    {
                        sb.AppendLine($"<ul>");
                        sb.AppendLine($"<li>記載なし</li>");
                        sb.AppendLine($"</ul>");
                    }

                    if (!string.IsNullOrEmpty(item.Meta.NegativePrompt))
                    {
                        sb.AppendLine($"<!-- wp:loos-hcb/code-block {{\"langType\":\"negativeprompt\",\"langName\":\"Negative Prompt\"}} -->");
                        sb.AppendLine($"<div class=\"hcb_wrap\"><pre class=\"prism undefined-numbers lang-negativeprompt\" data-lang=\"Negative Prompt\">");
                        sb.AppendLine($"<code>{item.Meta.NegativePrompt.HtmlCodeEscape()}</code>");
                        sb.AppendLine($"</pre></div>");
                        sb.AppendLine($"<!-- /wp:loos-hcb/code-block -->");
                    }
                    else
                    {
                        sb.AppendLine($"<ul>");
                        sb.AppendLine($"<li>記載なし</li>");
                        sb.AppendLine($"</ul>");
                    }
                }
                sb.AppendLine($"");
                imageid++;
            }
            return sb.ToString();
        }
        #endregion

        #region 記事タイプ2の取得(画像ありリスト)
        /// <summary>
        /// 記事タイプ2の取得(画像ありリスト)
        /// </summary>
        /// <returns>記事</returns>
        private string GetArticleType2()
        {
            var first = this.SearchResults.Items.ElementAt(0);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!-- wp:cover {\"url\":" +
                $"\"{first.Url}\"," +
                "\"alt\":\"トップ画像\",\"dimRatio\":10,\"focalPoint\":{\"x\":0.5,\"y\":0.95},\"minHeight\":840,\"minHeightUnit\":\"px\",\"contentPosition\":" +
                "\"bottom center\",\"align\":\"full\",\"style\":{\"spacing\":{\"padding\":{\"top\":\"var:preset|spacing|50\",\"bottom\":\"var:preset|spacing|50\",\"left\":\"var:preset|spacing|50\"," +
                "\"right\":\"var:preset|spacing|50\"},\"margin\":{\"top\":\"0\",\"bottom\":\"0\"}}},\"layout\":{\"type\":\"constrained\"}} -->");

            sb.AppendLine("<div class=\"wp-block-cover alignfull has-custom-content-position is-position-bottom-center\" " +
                "style=\"margin-top:0;margin-bottom:0;padding-top:var(--wp--preset--spacing--50);padding-right:var(--wp--preset--spacing--50);" +
                "padding-bottom:var(--wp--preset--spacing--50);padding-left:var(--wp--preset--spacing--50);min-height:840px\">" +
                "<span aria-hidden=\"true\" class=\"wp-block-cover__background has-background-dim-10 has-background-dim\"></span>" +
                "<img class=\"wp-block-cover__image-background\" alt=\"トップ画像\" " +
                $"src=\"{first.Url}\" " +
                "style=\"object-position:50% 95%\" data-object-fit=\"cover\" data-object-position=\"50% 95%\"/>" +
                "<div class=\"wp-block-cover__inner-container\"><!-- wp:group {\"align\":\"wide\",\"layout\":{\"type\":\"constrained\",\"justifyContent\":\"left\"}} -->");
            sb.AppendLine("<div class=\"wp-block-group alignwide\"><!-- wp:heading {\"textAlign\":\"left\",\"fontSize\":\"xx-large\"} -->");
            sb.AppendLine("<h2 class=\"wp-block-heading has-text-align-left has-xx-large-font-size\">" +
                $"Civitai速報！{GetPeriod()}イメージランキング" +
                "</h2>");
            sb.AppendLine("<!-- /wp:heading -->");

            sb.AppendLine("");
            sb.AppendLine("<!-- wp:paragraph -->");
            sb.AppendLine("<p>直近で最も反応があったAI生成画像を集めました</p>");

            DateTime start = (from x in this.SearchResults.Items
                              select x.CreatedAt).Min();
            DateTime end = (from x in this.SearchResults.Items
                              select x.CreatedAt).Max();
            sb.AppendLine($"対象期間 {start.ToString("yyyy/MM/dd")} ～ {end.ToString("yyyy/MM/dd")}");
            sb.AppendLine($"更新日 {DateTime.Today.ToString("yyyy/MM/dd(ddd)")}");
            sb.AppendLine("<!-- /wp:paragraph -->");
            sb.AppendLine("");


            sb.AppendLine("<!-- wp:buttons -->");
            sb.AppendLine("<div class=\"wp-block-buttons\"><!-- wp:button -->");
            sb.AppendLine("<div class=\"wp-block-button\"><a class=\"wp-block-button__link wp-element-button\" href=\"https://civitai.com/\">Civitai</a></div>");
            sb.AppendLine("<!-- /wp:button --></div>");
            sb.AppendLine("<!-- /wp:buttons --></div>");
            sb.AppendLine("<!-- /wp:group --></div></div>");
            sb.AppendLine("<!-- /wp:cover -->");
            sb.AppendLine("");

            int i = 1;
            foreach (var item in this.SearchResults.Items)
            {
                string style = i % 2 == 0 ? "is-style-section-5" : "is-style-default";
                sb.AppendLine("<!-- wp:group {\"align\":\"full\",\"className\":" +
                    $"\"{style}\"" +
                    ",\"style\":{\"spacing\":{\"padding\":{\"top\":\"var:preset|spacing|50\",\"bottom\":\"var:preset|spacing|50\"},\"margin\":{\"top\":\"0\",\"bottom\":\"0\"}}},\"layout\":{\"type\":\"constrained\"}} -->");
                sb.AppendLine($"<div class=\"wp-block-group alignfull {style}\" " +
                    "style=\"margin-top:0;margin-bottom:0;padding-top:var(--wp--preset--spacing--50);padding-bottom:var(--wp--preset--spacing--50)\">" +
                    "<!-- wp:columns {\"align\":\"wide\",\"style\":{\"spacing\":{\"blockGap\":{\"top\":\"var:preset|spacing|60\",\"left\":\"var:preset|spacing|80\"}}}} -->");
                sb.AppendLine("<div class=\"wp-block-columns alignwide\"><!-- wp:column {\"verticalAlignment\":\"center\",\"width\":\"50%\"} -->");
                sb.AppendLine("<div class=\"wp-block-column is-vertically-aligned-center\" style=\"flex-basis:50%\"><!-- wp:heading {\"className\":\"wp-block-heading\"} -->");
                sb.AppendLine("");
                sb.AppendLine($"<h2 class=\"wp-block-heading\">No.{i++}   作者 {item.Username}</h2>");
                sb.AppendLine("<!-- /wp:heading -->");
                sb.AppendLine("");
                sb.AppendLine("");
                sb.AppendLine("<!-- wp:paragraph -->");
                sb.AppendLine($"<p>作成日:{item.CreatedAt.ToString("yyyy/MM/dd")}</p>");
                sb.AppendLine("<!-- /wp:paragraph -->");
                sb.AppendLine("");
                if (item.Meta != null)
                {
                    if (!string.IsNullOrEmpty(item.Meta.Prompt))
                    {
                        sb.AppendLine($"<!-- wp:loos-hcb/code-block {{\"langType\":\"prompt\",\"langName\":\"Prompt\"}} -->");
                        sb.AppendLine($"<div class=\"hcb_wrap\"><pre class=\"prism undefined-numbers lang-prompt\" data-lang=\"Prompt\">");
                        sb.AppendLine($"<code>{item.Meta.Prompt.HtmlCodeEscape()}</code>");
                        sb.AppendLine($"</pre></div>");
                        sb.AppendLine($"<!-- /wp:loos-hcb/code-block -->");
                        sb.AppendLine("");
                    }
                    if (!string.IsNullOrEmpty(item.Meta.NegativePrompt))
                    {
                        sb.AppendLine($"<!-- wp:loos-hcb/code-block {{\"langType\":\"negativeprompt\",\"langName\":\"Negative Prompt\"}} -->");
                        sb.AppendLine($"<div class=\"hcb_wrap\"><pre class=\"prism undefined-numbers lang-negativeprompt\" data-lang=\"Negative Prompt\">");
                        sb.AppendLine($"<code>{item.Meta.NegativePrompt.HtmlCodeEscape()}</code>");
                        sb.AppendLine($"</pre></div>");
                        sb.AppendLine($"<!-- /wp:loos-hcb/code-block -->");
                        sb.AppendLine("");
                    }

                    if (!string.IsNullOrEmpty(item.Meta.Model))
                    {
                        sb.AppendLine("<!-- wp:paragraph -->");
                        sb.AppendLine($"<p>モデル:{item.Meta.Model}</p>");
                        sb.AppendLine("<!-- /wp:paragraph -->");
                        sb.AppendLine("");

                    }
                    sb.AppendLine("");

                }
                else
                {
                    sb.AppendLine("<!-- wp:paragraph {\"fontSize\":\"medium\"} -->");
                    sb.AppendLine("<p class=\"has-medium-font-size\">メタデータが存在しませんでした。</p>");
                    sb.AppendLine("<!-- /wp:paragraph -->");
                }
                sb.AppendLine("</div>");

                sb.AppendLine("<!-- /wp:column -->");
                sb.AppendLine("");
                sb.AppendLine("<!-- wp:column {\"verticalAlignment\":\"center\",\"width\":\"50%\",\"layout\":{\"type\":\"default\"}} -->");
                sb.AppendLine("<div class=\"wp-block-column is-vertically-aligned-center\" style=\"flex-basis:50%\">");

                string ext = System.IO.Path.GetExtension(item.Url).ToLower();
                if (ext.Equals(".mp4"))
                {
                    sb.AppendLine($"<!-- wp:video -->");
                    sb.AppendLine($"<figure class=\"wp-block-video\"><video controls src=\"{item.Url}\"></video></figure>");
                    sb.AppendLine($"<!-- /wp:video -->");
                }
                else
                {

                    sb.AppendLine($"<!-- wp:image {{\"aspectRatio\":\"1\",\"scale\":\"cover\",\"sizeSlug\":\"full\"}} -->");
                    sb.AppendLine($"<figure class=\"wp-block-image size-full\"><img src=\"{item.Url}\" alt=\"{item.Url}\" style=\"aspect-ratio:1;object-fit:cover\"/></figure>");
                    sb.AppendLine($"<!-- /wp:image -->");
                }

                sb.AppendLine("</div>");
                sb.AppendLine("<!-- /wp:column --></div>");
                sb.AppendLine("<!-- /wp:columns --></div>");
                sb.AppendLine("<!-- /wp:group -->");
                sb.AppendLine("");
            }


            sb.AppendLine("");
            sb.AppendLine("");



            return sb.ToString();
        }
        #endregion

        #region 説明の取得
        /// <summary>
        /// 説明の取得
        /// </summary>
        /// <returns>説明</returns>
        public string GetDescription()
        {
            return GetTitle();
        }
        #endregion

        #region 要約の取得
        /// <summary>
        /// 要約の取得
        /// </summary>
        /// <returns>要約</returns>
        public string GetExcerpt()
        {
            return GetTitle();
        }
        #endregion
    }
}
