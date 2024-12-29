using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WPManager.Models.Civitai.Models
{
    public class CvsMetadataM : BindableBase
    {
        #region The total number of items available[TotalItems]プロパティ
        /// <summary>
        /// The total number of items available[TotalItems]プロパティ用変数
        /// </summary>
        long _TotalItems = 0;
        /// <summary>
        /// The total number of items available[TotalItems]プロパティ
        /// </summary>
        [JsonPropertyName("totalItems")]
        public long TotalItems
        {
            get
            {
                return _TotalItems;
            }
            set
            {
                if (!_TotalItems.Equals(value))
                {
                    _TotalItems = value;
                    RaisePropertyChanged("TotalItems");
                }
            }
        }
        #endregion

        #region The the current page you are at[CurrentPage]プロパティ
        /// <summary>
        /// The the current page you are at[CurrentPage]プロパティ用変数
        /// </summary>
        long _CurrentPage = new long();
        /// <summary>
        /// The the current page you are at[CurrentPage]プロパティ
        /// </summary>
        [JsonPropertyName("currentPage")]
        public long CurrentPage
        {
            get
            {
                return _CurrentPage;
            }
            set
            {
                if (!_CurrentPage.Equals(value))
                {
                    _CurrentPage = value;
                    RaisePropertyChanged("CurrentPage");
                }
            }
        }
        #endregion

        #region The the size of the batch[PageSize]プロパティ
        /// <summary>
        /// The the size of the batch[PageSize]プロパティ用変数
        /// </summary>
        long _PageSize = new long();
        /// <summary>
        /// The the size of the batch[PageSize]プロパティ
        /// </summary>
        [JsonPropertyName("pageSize")]
        public long PageSize
        {
            get
            {
                return _PageSize;
            }
            set
            {
                if (!_PageSize.Equals(value))
                {
                    _PageSize = value;
                    RaisePropertyChanged("PageSize");
                }
            }
        }
        #endregion

        #region The total number of pages[TotalPages]プロパティ
        /// <summary>
        /// The total number of pages[TotalPages]プロパティ用変数
        /// </summary>
        long _TotalPages = new long();
        /// <summary>
        /// The total number of pages[TotalPages]プロパティ
        /// </summary>
        [JsonPropertyName("totalPages")]
        public long TotalPages
        {
            get
            {
                return _TotalPages;
            }
            set
            {
                if (!_TotalPages.Equals(value))
                {
                    _TotalPages = value;
                    RaisePropertyChanged("TotalPages");
                }
            }
        }
        #endregion

        #region The url to get the next batch of items[NextPage]プロパティ
        /// <summary>
        /// The url to get the next batch of items[NextPage]プロパティ用変数
        /// </summary>
        string _NextPage = string.Empty;
        /// <summary>
        /// The url to get the next batch of items[NextPage]プロパティ
        /// </summary>
        [JsonPropertyName("nextPage")]
        public string NextPage
        {
            get
            {
                return _NextPage;
            }
            set
            {
                if (_NextPage == null || !_NextPage.Equals(value))
                {
                    _NextPage = value;
                    RaisePropertyChanged("NextPage");
                }
            }
        }
        #endregion

        #region The url to get the previous batch of items[PrevPage]プロパティ
        /// <summary>
        /// The url to get the previous batch of items[PrevPage]プロパティ用変数
        /// </summary>
        string _PrevPage = string.Empty;
        /// <summary>
        /// The url to get the previous batch of items[PrevPage]プロパティ
        /// </summary>
        [JsonPropertyName("prevPage")]
        public string PrevPage
        {
            get
            {
                return _PrevPage;
            }
            set
            {
                if (_PrevPage == null || !_PrevPage.Equals(value))
                {
                    _PrevPage = value;
                    RaisePropertyChanged("PrevPage");
                }
            }
        }
        #endregion

        #region 次のカーソル[NextCursor]プロパティ
        /// <summary>
        /// 次のカーソル[NextCursor]プロパティ用変数
        /// </summary>
        string _NextCursor = string.Empty;
        /// <summary>
        /// 次のカーソル[NextCursor]プロパティ
        /// </summary>
        [JsonPropertyName("nextCursor")]
        public string NextCursor
        {
            get
            {
                return _NextCursor;
            }
            set
            {
                if (_NextCursor == null || !_NextCursor.Equals(value))
                {
                    _NextCursor = value;
                    RaisePropertyChanged("NextCursor");
                }
            }
        }
        #endregion
    }
}
