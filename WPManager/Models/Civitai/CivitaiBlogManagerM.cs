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
        #region エンドポイント
        /// <summary>
        /// エンドポイント
        /// </summary>
        const string EndPoint = "https://civitai.com/api/v1/models";
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
            this.Article.Title = CreateTitle();
            this.Article.Slug = CreateSlug();
            this.Article.Content = CreateArticle();
            this.Article.Description = CreateTitle();
            this.Article.Excerpt = CreateTitle();
            RaisePropertyChanged("Article");
        }
        #endregion

        #region スラッグを作成する
        /// <summary>
        /// スラッグを作成する
        /// </summary>
        /// <returns>スラッグ</returns>
        private string CreateSlug()
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

            return slug;
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

            return title;
        }
        #endregion

        #region 記事の作成処理
        /// <summary>
        /// 記事の作成処理
        /// </summary>
        /// <returns>記事</returns>
        private string CreateArticle()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<h2>{CreateTitle()}</h2>");
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
    }
}
