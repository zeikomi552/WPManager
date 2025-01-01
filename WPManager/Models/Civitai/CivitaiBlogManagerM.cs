using DryIoc;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPManager.Models.Civitai.Enums;
using WPManager.Models.Civitai.Models;

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
    }
}
