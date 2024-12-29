using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        public async void Search()
        {
            var client = new CivitaiClient("https://civitai.com/api/v1/models");
            this.SearchResults = await client.ModelSearch(this.SearchCondition.RequestQuery);
        }
    }
}
