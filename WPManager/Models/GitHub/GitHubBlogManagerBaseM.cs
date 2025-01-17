using ControlzEx.Standard;
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
using WPManager.Models.GitHub.Enums;

namespace WPManager.Models.GitHub
{
    public class GitHubBlogManagerBaseM : BaseBlogManagerM
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

        #region 検索条件
        /// <summary>
        /// 検索条件
        /// </summary>
        GitHubDataObjectM _SearchCondition = new GitHubDataObjectM();
        /// <summary>
        /// 検索条件
        /// </summary>
        public GitHubDataObjectM SearchCondition
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

        #region 検索結果をリポジトリ形式で保管するリスト
        /// <summary>
        /// 検索結果をリポジトリ形式で保管するリスト
        /// </summary>
        ObservableCollection<Repository> _SearchResults = new ObservableCollection<Repository>();
        /// <summary>
        /// 検索結果をリポジトリ形式で保管するリスト
        /// </summary>
        public ObservableCollection<Repository> SearchResults
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

        #region 選択中のリポジトリ
        /// <summary>
        /// 選択中のリポジトリ
        /// </summary>
        Repository _SelectedSearchRepository = new Repository();
        /// <summary>
        /// 選択中のリポジトリ
        /// </summary>
        public Repository SelectedSearchRepository
        {
            get
            {
                return _SelectedSearchRepository;
            }
            set
            {
                if (_SelectedSearchRepository == null || !_SelectedSearchRepository.Equals(value))
                {
                    this.RepositoryDetail = _SelectedSearchRepository = value;
                    this.UserDetail = _SelectedSearchRepository.Owner ?? new User();
                    RaisePropertyChanged("SelectedSearchRepository");
                }
            }
        }
        #endregion

        #region リポジトリ検索結果
        /// <summary>
        /// リポジトリ検索結果
        /// 単体で検索した際の結果格納場所
        /// </summary>
        Repository _RepositoryDetail = new Repository();
        /// <summary>
        /// リポジトリ検索結果
        /// </summary>
        public Repository RepositoryDetail
        {
            get
            {
                return _RepositoryDetail;
            }
            set
            {
                if (_RepositoryDetail == null || !_RepositoryDetail.Equals(value))
                {
                    _RepositoryDetail = value;
                    RaisePropertyChanged("RepositoryDetail");
                }
            }
        }
        #endregion

        #region ユーザー情報の詳細
        /// <summary>
        /// ユーザー情報の詳細
        /// </summary>
        User _UserDetail = new User();
        /// <summary>
        /// ユーザー情報の詳細
        /// </summary>
        public User UserDetail
        {
            get
            {
                return _UserDetail;
            }
            set
            {
                if (_UserDetail == null || !_UserDetail.Equals(value))
                {
                    _UserDetail = value;
                    RaisePropertyChanged("UserDetail");
                }
            }
        }
        #endregion

        #region 指定したページ数まで検索を実行する
        /// <summary>
        /// 指定したページ数まで検索を実行する
        /// 1から始まる
        /// </summary>
        /// <param name="pagemax">ページ最大値</param>
        /// <returns>リポジトリデータ</returns>
        protected async Task<List<Repository>> SearchPageMax(int pagemax)
        {
            List<Repository> list = new List<Repository>();
            for (int page = 1; page <= pagemax; page++)
            {
                var result = await Search(page);

                list.AddRange(result.Items.ToList<Repository>());
            }

            return list;
        }
        #endregion

        public async Task<bool> SearchMaxSync(int page_max)
        {
            try
            {
                List<Repository> list = new List<Repository>();
                for (int page = 1; page <= page_max; page++)
                {
                    var result = await Search(page);

                    list.AddRange(result.Items.ToList<Repository>());
                }

                this.SearchResults = new ObservableCollection<Repository>(list);

                if (this.SearchResults.Count > 0)
                {
                    // 記事に関する各要素をセット
                    SetArticleInfo();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        #region 検索の実行処理
        /// <summary>
        /// 検索の実行処理
        /// </summary>
        public virtual async void Search()
        {
            var result = await Search(0);

            this.SearchResults = new ObservableCollection<Repository>(result.Items.ToList<Repository>());

            // nullチェック
            if (this.SearchResults != null)
            {
                // 記事に関する各要素をセット
                SetArticleInfo();
            }
        }
        #endregion

        #region 検索の実行処理
        /// <summary>
        /// 検索の実行処理
        /// </summary>
        public virtual async Task<bool> SearchSync(int page = 0)
        {
            try
            {
                var result = await Search(page);

                this.SearchResults = new ObservableCollection<Repository>(result.Items.ToList<Repository>());

                // nullチェック
                if (this.SearchResults != null)
                {
                    // 記事に関する各要素をセット
                    SetArticleInfo();
                }
                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

        }
        #endregion

        #region 検索処理
        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="page">検索するページ</param>
        protected async Task<SearchRepositoryResult> Search(int page)
        {
            // GitHub Clientの作成
            var client = new GitHubClient(new ProductHeaderValue(this.GitHubParameter.ProductName));

            // トークンの取得
            var tokenAuth = new Credentials(this.GitHubParameter.AccessToken);
            client.Credentials = tokenAuth;

            SearchRepositoriesRequest request = new SearchRepositoriesRequest();
#pragma warning disable CS0618 // 型またはメンバーが旧型式です

            // 値を持っているかどうかのチェック
            request.Created = new DateRange(this.SearchCondition.SearchFrom, this.SearchCondition.SearchTo);

            // スターの数
            request.Stars = new Octokit.Range(1, int.MaxValue);

            // 読み込むページ
            request.Page = page;

            // スターの数でソート
            request.SortField = RepoSearchSort.Stars;

            if (this.SearchCondition.SelectedLanguage.HasValue)
            {
                request.Language = this.SearchCondition.SelectedLanguage;
            }

            // 降順でソート
            request.Order = SortDirection.Descending;

            var query = request.ToString();
#pragma warning restore CS0618 // 型またはメンバーが旧型式です
            return await client.Search.SearchRepo(request);

        }
        #endregion

        public async void SearchUserSync()
        {
            try
            {
                var userName = this.SelectedSearchRepository.Owner.Login;
                this.UserDetail = await SearchUser(userName);
            }
            catch
            {

            }
        }



        /// <summary>
        /// ユーザーの検索処理
        /// </summary>
        /// <param name="username">ユーザー名</param>
        /// <returns>ユーザー検索結果</returns>
        private async Task<User> SearchUser(string username)
        {
            // GitHub Clientの作成
            var client = new GitHubClient(new ProductHeaderValue(this.GitHubParameter.ProductName));

            // トークンの取得
            var tokenAuth = new Credentials(this.GitHubParameter.AccessToken);
            client.Credentials = tokenAuth;
            return await client.User.Get(username);
        }

        public async void SearchRepositorySync()
        {
            try
            {
                var ownerName = this.SelectedSearchRepository.Owner.Login;
                var repositoryName = this.SelectedSearchRepository.Name;

                this.RepositoryDetail = await SearchRepository(ownerName, repositoryName);
            }
            catch
            {
            }
        }

        /// <summary>
        /// リポジトリ情報の取得
        /// </summary>
        /// <param name="ownerName">オーナー名</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <returns>リポジトリ</returns>
        private async Task<Repository> SearchRepository(string ownerName, string repositoryName)
        {
            // GitHub Clientの作成
            var client = new GitHubClient(new ProductHeaderValue(this.GitHubParameter.ProductName));

            // トークンの取得
            var tokenAuth = new Credentials(this.GitHubParameter.AccessToken);
            client.Credentials = tokenAuth;
            return await client.Repository.Get(ownerName, repositoryName);
        }

        /// <summary>
        /// 組織情報の取得
        /// </summary>
        /// <param name="orgnaizationName">組織名</param>
        /// <returns>組織情報</returns>
        public async Task<Organization> SearchOrgnaization(string orgnaizationName)
        {
            // GitHub Clientの作成
            var client = new GitHubClient(new ProductHeaderValue(this.GitHubParameter.ProductName));

            // トークンの取得
            var tokenAuth = new Credentials(this.GitHubParameter.AccessToken);
            client.Credentials = tokenAuth;
            return await client.Organization.Get(orgnaizationName);
        }

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
        protected virtual string GetArticleType1()
        {
            return string.Empty;
        }
        #endregion

        #region 記事作成処理
        /// <summary>
        /// 記事作成処理
        /// </summary>
        /// <returns>記事</returns>
        protected virtual string GetArticleType2()
        {
            return string.Empty;
        }
        #endregion

        #region スラッグを作成する
        /// <summary>
        /// スラッグを作成する
        /// </summary>
        /// <returns>スラッグ</returns>
        protected override string GetSlug()
        {
            return string.Empty;
        }
        #endregion

        #region タイトルの作成処理
        /// <summary>
        /// タイトルの作成処理
        /// </summary>
        /// <returns>タイトル</returns>
        protected override string GetTitle()
        {
            return string.Empty;

        }
        #endregion

        #region 詳細の作成処理
        /// <summary>
        /// 詳細の作成処理
        /// </summary>
        /// <returns>詳細</returns>
        protected override string GetDescription()
        {
            return string.Empty;
        }
        #endregion

        #region 要約の作成処理
        /// <summary>
        /// 要約の作成処理
        /// </summary>
        /// <returns>要約</returns>
        protected override string GetExcerpt()
        {
            return string.Empty;

        }
        #endregion


    }
}
