using Octokit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPManager.Models;

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

            // 記事の作成
            //this.Article = RepositorySearchResultM.GetArticle(this.SearchDateRange, request.Language, this.SearchResult);
        }
        #endregion

        public void LanguageClear()
        {
            this.SelectedLanguage = null;
        }
    }
}
