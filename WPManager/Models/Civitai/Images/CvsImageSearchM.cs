using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPManager.Models.Civitai.Enums;

namespace WPManager.Models.Civitai.Images
{
    public class CvsImageSearchM :BindableBase
    {
        #region The number of results to be returned per page. This can be a number between 1 and 200. By default, each page will return 100 results[Limit]プロパティ
        /// <summary>
        /// The number of results to be returned per page. This can be a number between 1 and 200. By default, each page will return 100 results[Limit]プロパティ用変数
        /// </summary>
        int? _Limit = 100;
        /// <summary>
        /// The number of results to be returned per page. This can be a number between 1 and 200. By default, each page will return 100 results[Limit]プロパティ
        /// </summary>
        public int? Limit
        {
            get
            {
                return _Limit;
            }
            set
            {
                if (_Limit == null || !_Limit.Equals(value))
                {
                    _Limit = value;
                    RaisePropertyChanged("Limit");
                }
            }
        }
        #endregion

        #region The ID of a post to get images from[PostId]プロパティ
        /// <summary>
        /// The ID of a post to get images from[PostId]プロパティ用変数
        /// </summary>
        Int64? _PostId = null;
        /// <summary>
        /// The ID of a post to get images from[PostId]プロパティ
        /// </summary>
        public Int64? PostId
        {
            get
            {
                return _PostId;
            }
            set
            {
                if (_PostId == null || !_PostId.Equals(value))
                {
                    _PostId = value;
                    RaisePropertyChanged("PostId");
                }
            }
        }
        #endregion

        #region The ID of a model to get images from (model gallery)[ModelId]プロパティ
        /// <summary>
        /// The ID of a model to get images from (model gallery)[ModelId]プロパティ用変数
        /// </summary>
        Int64? _ModelId = null;
        /// <summary>
        /// The ID of a model to get images from (model gallery)[ModelId]プロパティ
        /// </summary>
        public Int64? ModelId
        {
            get
            {
                return _ModelId;
            }
            set
            {
                if (_ModelId == null || !_ModelId.Equals(value))
                {
                    _ModelId = value;
                    RaisePropertyChanged("ModelId");
                }
            }
        }
        #endregion

        #region The ID of a model version to get images from (model gallery filtered to version)[ModelVersionId]プロパティ
        /// <summary>
        /// The ID of a model version to get images from (model gallery filtered to version)[ModelVersionId]プロパティ用変数
        /// </summary>
        Int64? _ModelVersionId = null;
        /// <summary>
        /// The ID of a model version to get images from (model gallery filtered to version)[ModelVersionId]プロパティ
        /// </summary>
        public Int64? ModelVersionId
        {
            get
            {
                return _ModelVersionId;
            }
            set
            {
                if (_ModelVersionId == null || !_ModelVersionId.Equals(value))
                {
                    _ModelVersionId = value;
                    RaisePropertyChanged("ModelVersionId");
                }
            }
        }
        #endregion

        #region The page from which to start fetching models[Page ]プロパティ
        /// <summary>
        /// The page from which to start fetching models[Page ]プロパティ用変数
        /// </summary>
        int? _Page = 1;
        /// <summary>
        /// The page from which to start fetching models[Page ]プロパティ
        /// </summary>
        public int? Page
        {
            get
            {
                return _Page;
            }
            set
            {
                if (_Page == null || !_Page.Equals(value))
                {
                    _Page = value;
                    RaisePropertyChanged("Page");
                }
            }
        }
        #endregion

        #region カーソルリスト[CursorList]プロパティ
        /// <summary>
        /// カーソルリスト[CursorList]プロパティ用変数
        /// </summary>
        Dictionary<string, string> _CursorList = new Dictionary<string, string>();
        /// <summary>
        /// カーソルリスト[CursorList]プロパティ
        /// </summary>
        public Dictionary<string, string> CursorList
        {
            get
            {
                return _CursorList;
            }
            set
            {
                if (_CursorList == null || !_CursorList.Equals(value))
                {
                    _CursorList = value;
                    RaisePropertyChanged("CursorList");
                }
            }
        }
        #endregion

        #region Search query to filter models by user[Username ]プロパティ
        /// <summary>
        /// Search query to filter models by user[Username ]プロパティ用変数
        /// </summary>
        string? _Username = null;
        /// <summary>
        /// Search query to filter models by user[Username ]プロパティ
        /// </summary>
        public string? Username
        {
            get
            {
                return _Username;
            }
            set
            {
                if (_Username == null || !_Username.Equals(value))
                {
                    _Username = value;
                    RaisePropertyChanged("Username");
                }
            }
        }
        #endregion

        #region The time frame in which the models will be sorted[Period]プロパティ
        /// <summary>
        /// The time frame in which the models will be sorted[Period]プロパティ用変数
        /// </summary>
        ModelPeriodEnum? _Period = ModelPeriodEnum.Empty;
        /// <summary>
        /// The time frame in which the models will be sorted[Period]プロパティ
        /// </summary>
        public ModelPeriodEnum? Period
        {
            get
            {
                return _Period;
            }
            set
            {
                if (_Period == null || !_Period.Equals(value))
                {
                    _Period = value;
                    RaisePropertyChanged("Period");
                }
            }
        }
        #endregion

        #region If false, will return safer images and hide models that don't have safe images[Nsfw]プロパティ
        /// <summary>
        /// If false, will return safer images and hide models that don't have safe images[Nsfw]プロパティ用変数
        /// </summary>
        bool? _Nsfw = null;
        /// <summary>
        /// If false, will return safer images and hide models that don't have safe images[Nsfw]プロパティ
        /// </summary>
        public bool? Nsfw
        {
            get
            {
                return _Nsfw;
            }
            set
            {
                if (_Nsfw == null || !_Nsfw.Equals(value))
                {
                    _Nsfw = value;
                    RaisePropertyChanged("Nsfw");
                }
            }
        }
        #endregion

        #region The order in which you wish to sort the results[Sort]プロパティ
        /// <summary>
        /// The order in which you wish to sort the results[Sort]プロパティ用変数
        /// </summary>
        ModelSortEnum2? _Sort = null;
        /// <summary>
        /// The order in which you wish to sort the results[Sort]プロパティ
        /// </summary>
        public ModelSortEnum2? Sort
        {
            get
            {
                return _Sort;
            }
            set
            {
                if (_Sort == null || !_Sort.Equals(value))
                {
                    _Sort = value;
                    RaisePropertyChanged("Sort");
                }
            }
        }
        #endregion

        #region GET Condition[RequestQuery]プロパティ
        /// <summary>
        /// GET Condition[RequestQuery]プロパティ用変数
        /// </summary>
        string _GetConditionQuery = string.Empty;
        /// <summary>
        /// GET Condition[RequestQuery]プロパティ
        /// </summary>
        public string RequestQuery
        {
            get
            {
                string query = string.Empty;

                query += $"limit={this.Limit}";
                if (this.PostId.HasValue) query += $"&postId={this.PostId.Value}";
                if (this.ModelId.HasValue) query += $"&modelId={this.ModelId.Value}";
                if (this.ModelVersionId.HasValue) query += $"&modelVersionId={this.ModelVersionId.Value}";
                if (!string.IsNullOrEmpty(this.Username)) query += $"&username={this.Username}";
                if (this.Period.HasValue && !this.Period.Equals(ModelPeriodEnum.Empty)) query += $"&period={this.Period.Value}";
                if (this.Nsfw.HasValue) query += $"&nsfw={this.Nsfw.Value}";
                if (this.Sort.HasValue && !this.Sort.Equals(ModelSortEnum2.Empty)) query += $"&sort={this.Sort.Value.ToString().Replace("_", "+")}";

                return "?" + query;
            }
        }
        #endregion

    }
}
