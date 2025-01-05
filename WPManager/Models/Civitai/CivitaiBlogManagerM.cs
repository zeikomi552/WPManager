using DryIoc;
using DryIoc.FastExpressionCompiler.LightExpression;
using Microsoft.Win32;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPManager.Common.Extensions;
using WPManager.Models.Civitai.Enums;
using WPManager.Models.Civitai.Models;
using WPManager.Models.Schedule;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace WPManager.Models.Civitai
{
    public class CivitaiBlogManagerM : BaseBlogManagerM
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
        const string EndPoint = "https://civitai.com/api/v1/models";
        #endregion

        #region 検索条件
        /// <summary>
        /// 検索条件
        /// </summary>
        CvsModelSearchM _SearchCondition = new CvsModelSearchM();
        /// <summary>
        /// 検索条件
        /// </summary>
        public CvsModelSearchM SearchCondition
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
        CvsModelM _SearchResults = new CvsModelM();
        /// <summary>
        /// 検索結果
        /// </summary>
        public CvsModelM SearchResults
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
                    //this.Article.Content = CreateArticle();
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
            this.SearchResults = await client.ModelSearch(this.SearchCondition.RequestQuery);
            SetArticleInfo();
        }
        #endregion

        #region 検索処理
        /// <summary>
        /// 検索処理
        /// </summary>
        public async Task<bool> SearchSync()
        {
            try
            {
                var client = new CivitaiClient(EndPoint);
                this.SearchResults = await client.ModelSearch(this.SearchCondition.RequestQuery);
                SetArticleInfo();

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        //public void SetScheduleCondition(string title, string slug, )
        //{
        //}

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

            if (!this.SearchCondition.Types.HasValue
                || this.SearchCondition.Types.Value == ModelTypeEnum.Empty)
            {
                slug = ($"{period}-civitai-alltypes");
            }
            else
            {
                slug = ($"{period}-civitai-{this.SearchCondition.Types.ToString()}");
            }

            slug = slug + this.ArticleType.ToString();
            return slug;
        }
        #endregion

        #region タイトルの作成処理
        /// <summary>
        /// タイトルの作成処理
        /// </summary>
        /// <returns>タイトル</returns>
        private string GetTitle()
        {
            string title = string.Empty;

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


            if (this.SearchCondition.Types.HasValue)
            {
                switch (this.SearchCondition.Types)
                {
                    case ModelTypeEnum.Checkpoint:
                        {
                            title = ($"{DateTime.Today.Year}年版 CIVITAI人気モデル速報！{period}ダウンロード数ランキング");
                            break;
                        }
                    case ModelTypeEnum.LORA:
                        {
                            title = ($"{DateTime.Today.Year}年版 CIVITAI人気{ModelTypeEnum.LORA.ToString()}速報！{period}ダウンロード数ランキング");
                            break;
                        }
                    default:
                        {
                            title = ($"{DateTime.Today.Year}年版 CIVITAI人気{this.SearchCondition.Types.ToString()}速報！{period}ダウンロード数ランキング");
                            break;
                        }
                }
            }
            else
            {
                title = ($"{DateTime.Today.Year}年版 CIVITAI人気ダウンロード速報！{period}ダウンロード数ランキング");
            }

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
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<h2>{GetTitle()}</h2>");
            sb.AppendLine($"<p>データ取得日 : {DateTime.Today.ToString("yyyy/MM/dd")}</p>");

            sb.AppendLine($"<table border=\"1\">");
            sb.AppendLine($"<thead>");
            sb.AppendLine($"<tr>");
            sb.AppendLine($"<th><center>順位</center><center>(DL数)</center></th>");
            sb.AppendLine($"<th>モデルID / 作者名<br>モデル名</th>");
            sb.AppendLine($"<th>モデルタイプ</th>");
            sb.AppendLine($"</tr>");
            sb.AppendLine($"</thead>");
            sb.AppendLine($"<tbody>");

            int rank = 1;
            foreach (var item in this.SearchResults.Items)
            {
                sb.AppendLine($"<tr>");
                sb.AppendLine($"<td><center>{rank++}位</center><center>({item.Stats.DownloadCount})</center></td>");
                sb.AppendLine($"<td>{item.Id} / <a href=\"https://civitai.com/user/{item.Creator.Username}/models\">{item.Creator.Username}</a>");
                sb.AppendLine($"<a href=\"https://civitai.com/models/{item.Id}\">{item.Name.Replace("|", "\\|")}</a></td>");
                sb.AppendLine($"<td>{item.Type}</td>");
                sb.AppendLine($"</tr>");
            }
            sb.AppendLine($"</tbody>");
            sb.AppendLine($"</table>");

            // ファイル出力処理
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
            var first_model = first.ModelVersions.FirstOrDefault();
            var first_image = first_model!.Images.FirstOrDefault();

            string first_image_url = first_image == null ? string.Empty : first_image.Url;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!-- wp:cover {\"url\":" +
                $"\"{first_image_url}\"," +
                "\"alt\":\"トップ画像\",\"dimRatio\":10,\"focalPoint\":{\"x\":0.5,\"y\":0.15},\"minHeight\":840,\"minHeightUnit\":\"px\",\"contentPosition\":" +
                "\"bottom center\",\"align\":\"full\",\"style\":{\"spacing\":{\"padding\":{\"top\":\"var:preset|spacing|50\",\"bottom\":\"var:preset|spacing|50\",\"left\":\"var:preset|spacing|50\"," +
                "\"right\":\"var:preset|spacing|50\"},\"margin\":{\"top\":\"0\",\"bottom\":\"0\"}}},\"layout\":{\"type\":\"constrained\"}} -->");

            sb.AppendLine("<div class=\"wp-block-cover alignfull has-custom-content-position is-position-bottom-center\" " +
                "style=\"margin-top:0;margin-bottom:0;padding-top:var(--wp--preset--spacing--50);padding-right:var(--wp--preset--spacing--50);" +
                "padding-bottom:var(--wp--preset--spacing--50);padding-left:var(--wp--preset--spacing--50);min-height:840px\">" +
                "<span aria-hidden=\"true\" class=\"wp-block-cover__background has-background-dim-10 has-background-dim\"></span>" +
                "<img class=\"wp-block-cover__image-background\" alt=\"トップ画像\" " +
                $"src=\"{first_image_url}\" " +
                "style=\"object-position:50% 15%\" data-object-fit=\"cover\" data-object-position=\"50% 15%\"/>" +
                "<div class=\"wp-block-cover__inner-container\"><!-- wp:group {\"align\":\"wide\",\"layout\":{\"type\":\"constrained\",\"justifyContent\":\"left\"}} -->");
            sb.AppendLine("<div class=\"wp-block-group alignwide\"><!-- wp:heading {\"textAlign\":\"left\",\"fontSize\":\"xx-large\"} -->");
            sb.AppendLine("<h2 class=\"wp-block-heading has-text-align-left has-xx-large-font-size\">" +
                $"Civitai速報！{GetPeriod()}イメージランキング" +
                "</h2>");
            sb.AppendLine("<!-- /wp:heading -->");

            sb.AppendLine("");
            sb.AppendLine("<!-- wp:paragraph -->");
            sb.AppendLine("<p>直近で最もダウンロードされたモデル(Checkpoint)を集めました。</p>");

            //DateTime start = (from x in this.SearchResults.Items
            //                  select x.).Min();
            //DateTime end = (from x in this.SearchResults.Items
            //                select x.CreatedAt).Max();
            //sb.AppendLine($"対象期間 {start.ToString("yyyy/MM/dd")} ～ {end.ToString("yyyy/MM/dd")}");
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
            int imageid = 1;
            foreach (var item in this.SearchResults.Items)
            {
                var curr_model = item.ModelVersions.FirstOrDefault();

                if (curr_model == null)
                    continue;

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
                sb.AppendLine($"<h2 class=\"wp-block-heading\">No.{i++}   作者 {item.Creator.Username}</h2>");
                sb.AppendLine("<!-- /wp:heading -->");
                sb.AppendLine("");
                sb.AppendLine("");
                sb.AppendLine("<!-- wp:paragraph -->");
                string date_txt = curr_model.PublishedAt.HasValue ? curr_model.PublishedAt.Value.ToString("yyyy/MM/dd") : string.Empty;
                sb.AppendLine($"作成日:{date_txt}<br>");
                sb.AppendLine($"モデル名:{item.Name}<br>");
                sb.AppendLine($"モデルバージョン:{curr_model.Name}<br>");
                sb.AppendLine("<!-- /wp:paragraph -->");
                sb.AppendLine("");

                var image = curr_model.Images.FirstOrDefault();

                sb.AppendLine("<!-- wp:buttons -->");
                sb.AppendLine("<div class=\"wp-block-buttons\"><!-- wp:button {\"fontSize\":\"small\"} -->");
                sb.AppendLine("<div class=\"wp-block-button has-custom-font-size has-small-font-size\"><a class=\"wp-block-button__link wp-element-button\" " +
                    $"href=\"https://civitai.com/models/{item.Id}/flux?modelVersionId={curr_model.Id}\">" +
                    "Civitai</a></div>");
                sb.AppendLine("<!-- /wp:button --></div>");
                sb.AppendLine("<!-- /wp:buttons -->");
                sb.AppendLine("");

                sb.AppendLine("</div>");

                sb.AppendLine("<!-- /wp:column -->");
                sb.AppendLine("");
                sb.AppendLine("<!-- wp:column {\"verticalAlignment\":\"center\",\"width\":\"50%\",\"layout\":{\"type\":\"default\"}} -->");
                sb.AppendLine("<div class=\"wp-block-column is-vertically-aligned-center\" style=\"flex-basis:50%\">");


                sb.AppendLine($"<!-- wp:gallery {{\"linkTo\":\"none\"}} -->");
                sb.AppendLine($"<figure class=\"wp-block-gallery has-nested-images columns-default is-cropped\">");

                int count = 0;
                foreach (var tmp in curr_model.Images)
                {
                    sb.AppendLine($"<!-- wp:image {{\"id\":{imageid},\"sizeSlug\":\"large\",\"linkDestination\":\"none\"}} -->");
                    sb.AppendLine($"<figure class=\"wp-block-image size-large\">");
                    sb.AppendLine($"<img src=\"{tmp.Url}\" alt=\"\" class=\"wp-image-{imageid}\"/>");
                    sb.AppendLine($"</figure>");
                    sb.AppendLine($"<!-- /wp:image -->");

                    if (count == 5)
                        break;

                    count++;
                }
                sb.AppendLine($"</figure>");
                sb.AppendLine($"<!-- /wp:gallery -->");
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


        #region 記事タイプ2の取得(画像ありリスト)
        /// <summary>
        /// 記事タイプ2の取得(画像ありリスト)
        /// </summary>
        /// <returns>記事</returns>
        private string GetArticleType3()
        {
            // ダイアログのインスタンスを生成
            var dialog = new SaveFileDialog();

            // ファイルの種類を設定
            dialog.Filter = "マークダウン (*.md)|*.md";

            StringBuilder sb = new StringBuilder();

            int rank = 1;
            int imageid = 1;
            foreach (var item in this.SearchResults.Items)
            {
                sb.AppendLine($"<h2> {rank++}位 {item.Id} {item.Name}</h2>");
                sb.AppendLine($"");
                sb.AppendLine($"<ul>");
                sb.AppendLine($"<li>Creator : {item.Creator.Username}</li>");
                sb.AppendLine($"<li>Download Count : {item.Stats.DownloadCount}</li>");
                //sb.AppendLine($"<li>Favorite Count : {item.Stats.FavoriteCount}</li>");
                sb.AppendLine($"<li>Comment Count : {item.Stats.CommentCount}</li>");
                sb.AppendLine($"<li><a href=\"https://civitai.com/models/{item.Id}\">Page Link</a></li>");
                sb.AppendLine($"</ul>");
                sb.AppendLine($"");
                foreach (var modelver in item.ModelVersions)
                {
                    sb.AppendLine($"<h3>ver : {modelver.Name}</h3>");
                    sb.AppendLine($"<ul>");
                    sb.AppendLine($"<li><a href=\"https://civitai.com/models/{item.Id}?modelVersionId={modelver.Id}\">ModelVersionURL</a></li>");
                    sb.AppendLine($"<li><a href=\"{modelver.DownloadUrl}\">Model Download</a></li>");
                    sb.AppendLine($"</ul>");

                    sb.AppendLine($"");
                    int count = 0;

                    sb.AppendLine($"<!-- wp:gallery {{\"linkTo\":\"none\"}} -->");
                    sb.AppendLine($"<figure class=\"wp-block-gallery has-nested-images columns-default is-cropped\">");

                    foreach (var image in modelver.Images)
                    {
                        //sb.AppendLine($"");
                        //sb.AppendLine($"{image.Nsfw}");

                        if (image.Meta != null)
                        {
                            //sb.AppendLine($"```");
                            //sb.AppendLine($"Prompt : {image.Meta.Prompt}");
                            //sb.AppendLine($"");
                            //sb.AppendLine($"Negative Prompt : {image.Meta.NegativPrompt}");
                            //sb.AppendLine($"```");
                            //sb.AppendLine($"");
                            //sb.AppendLine($"");
                            sb.AppendLine($"<!-- wp:image {{\"id\":{imageid},\"sizeSlug\":\"large\",\"linkDestination\":\"none\"}} -->");
                            sb.AppendLine($"<figure class=\"wp-block-image size-large\">");
                            sb.AppendLine($"<img src=\"{image.Url}\" alt=\"\" class=\"wp-image-{imageid}\"/>");
                            sb.AppendLine($"</figure>");
                            sb.AppendLine($"<!-- /wp:image -->");

                            //sb.AppendLine($"<img alt=\"{image.Url}\" src=\"{image.Url}\" width=\"20%\">");

                            imageid++;
                            //sb.AppendLine($"");
                            if (count++ >= 2) break;
                            //break;
                        }
                    }
                    sb.AppendLine($"</figure>");
                    sb.AppendLine($"<!-- /wp:gallery -->");
                    sb.AppendLine($"");

                    break; // 先頭のモデルでループを抜ける
                }
                sb.AppendLine($"");
            }
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

        #region 検索と投稿を実行する
        /// <summary>
        /// 検索と投稿を実行する
        /// </summary>
        /// <param name="schdule_item">スケジュール設定データ</param>
        /// <param name="wpPrame">WPコンフィグ情報</param>
        public async void SearchAndPost(ScheduleM schdule_item, WPParameterM wpPrame)
        {
            try
            {
                CivitaiBlogManagerM civitai_model = new CivitaiBlogManagerM();

                // 基本検索条件をセット
                civitai_model.SearchCondition.Types = ModelTypeEnum.Checkpoint;
                civitai_model.SearchCondition.Sort = ModelSortEnum.Most_Downloaded;
                civitai_model.SearchCondition.Period = schdule_item.CivitaiPeriod;

                // 投稿記事 or 固定ページを設定 
                civitai_model.PostOrPage = schdule_item.PostPageType;

                // 記事Idをセット(新規作成の場合は0)
                civitai_model.Article.PostId = schdule_item.ArticleId;

                // 記事タイプ 1:簡素バージョン 2:豪華バージョン
                civitai_model.ArticleType = schdule_item.ArticleType == 1 ? CivitaiArticleType.Type1 : CivitaiArticleType.Type2;

                // 検索の実行
                bool ret = await civitai_model.SearchSync();

                if (ret)
                {
                    // ポストの実行
                    civitai_model.Post(wpPrame);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
    }
}
