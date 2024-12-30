using DryIoc;
using Markdig;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPManager.Models;
using WPManager.Models.Civitai;
using WPManager.Models.Civitai.Models;

namespace WPManager.ViewModels.UserControls
{
    public class ucCivitaiVViewModel : BindableBase
    {
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
                    this.Article.Content = CreateArticle();
                }
            }
        }
        #endregion

        #region ブログ記事
        /// <summary>
        /// ブログ記事
        /// </summary>
        WPDataObjectM _Article = new WPDataObjectM();
        /// <summary>
        /// ブログ記事
        /// </summary>
        public WPDataObjectM Article
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

        #region Wordpress接続用パラメーター
        /// <summary>
        /// Wordpress接続用パラメーター
        /// </summary>
        IWPParameterM? _WPParameter;
        /// <summary>
        /// Wordpress接続用パラメーター
        /// </summary>
        public IWPParameterM? WPParameter
        {
            get
            {
                return _WPParameter;
            }
            set
            {
                if (_WPParameter == null || !_WPParameter.Equals(value))
                {
                    _WPParameter = value;
                    RaisePropertyChanged("WPParameter");
                }
            }
        }
        #endregion
        public ucCivitaiVViewModel(IGlobalConfigM gConfig)
        {
            this.WPParameter = gConfig.WPConfig;
        }

        public async void Search()
        {
            var client = new CivitaiClient("https://civitai.com/api/v1/models");
            this.SearchResults = await client.ModelSearch(this.SearchCondition.RequestQuery);

            this.Article.Title = CreateTitle();
            this.Article.Slug = CreateSlug();
            this.Article.Content = CreateArticle();
            this.Article.Description = CreateTitle();
            this.Article.Excerpt = CreateTitle();
            RaisePropertyChanged("Article");
        }


        private string CreateSlug()
        {
            string slug = string.Empty;

            string period = "all";
            if (this.SearchCondition.Period.HasValue)
            {
                switch (this.SearchCondition.Period)
                {
                    case Models.Civitai.Enums.ModelPeriodEnum.Year:
                        {
                            period = "yearly";
                            break;
                        }
                    case Models.Civitai.Enums.ModelPeriodEnum.Month:
                        {
                            period = "monthly";
                            break;
                        }
                    case Models.Civitai.Enums.ModelPeriodEnum.Week:
                        {
                            period = "weekly";
                            break;
                        }
                    case Models.Civitai.Enums.ModelPeriodEnum.Day:
                        {
                            period = "daily";
                            break;
                        }
                    case Models.Civitai.Enums.ModelPeriodEnum.AllTime:
                    case Models.Civitai.Enums.ModelPeriodEnum.Empty:
                        {
                            period = "allperiod";
                            break;
                        }
                }
            }

            if (!this.SearchCondition.Types.HasValue
                || this.SearchCondition.Types.Value == Models.Civitai.Enums.ModelTypeEnum.Empty)
            {
                slug = ($"{period}-civitai-alltypes");
            }
            else
            {
                slug = ($"{period}-civitai-{this.SearchCondition.Types.ToString()}");
            }

            return slug;
        }

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
                    case Models.Civitai.Enums.ModelPeriodEnum.Year:
                        {
                            period = "年間";
                            break;
                        }
                    case Models.Civitai.Enums.ModelPeriodEnum.Month:
                        {
                            period = "月間";
                            break;
                        }
                    case Models.Civitai.Enums.ModelPeriodEnum.Week:
                        {
                            period = "週間";
                            break;
                        }
                    case Models.Civitai.Enums.ModelPeriodEnum.Day:
                        {
                            period = "デイリー";
                            break;
                        }
                    case Models.Civitai.Enums.ModelPeriodEnum.AllTime:
                    case Models.Civitai.Enums.ModelPeriodEnum.Empty:
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
                    case Models.Civitai.Enums.ModelTypeEnum.Checkpoint:
                        {
                            title = ($"{DateTime.Today.Year}年版 CIVITAI人気モデル速報！{period}ダウンロード数ランキング");
                            break;
                        }
                    case Models.Civitai.Enums.ModelTypeEnum.LORA:
                        {
                            title = ($"{DateTime.Today.Year}年版 CIVITAI人気{Models.Civitai.Enums.ModelTypeEnum.LORA.ToString()}速報！{period}ダウンロード数ランキング");
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

            //convert Mark down to html and set to mdContents
            //Markdig.MarkdownPipeline markdownPipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();

            //string mdContents = Markdown.ToHtml(sb.ToString(), markdownPipeline);

            // ファイル出力処理
            return sb.ToString();
        }

        public void Post()
        {
            Task.Run(() =>
            {
                this.WPParameter!.CreateOrUpdatePost(this.Article).Wait();
            });
        }
    }
}
