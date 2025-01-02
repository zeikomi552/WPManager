using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPManager.Models;
using WPManager.Models.Civitai;

namespace WPManager.ViewModels.UserControls
{
    public class ucCivitaiImageVViewModel : BindableBase
    {
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

        #region ブログ管理オブジェクト
        /// <summary>
        /// ブログ管理オブジェクト
        /// </summary>
        CivitaiBlogImageManagerM _BlogManager = new CivitaiBlogImageManagerM();
        /// <summary>
        /// ブログ管理オブジェクト
        /// </summary>
        public CivitaiBlogImageManagerM BlogManager
        {
            get
            {
                return _BlogManager;
            }
            set
            {
                if (_BlogManager == null || !_BlogManager.Equals(value))
                {
                    _BlogManager = value;
                    RaisePropertyChanged("BlogManager");
                }
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="gConfig">コンフィグデータ</param>
        public ucCivitaiImageVViewModel(IGlobalConfigM gConfig)
        {
            this.WPParameter = gConfig.WPConfig;
        }
        #endregion

        public void Search()
        {
            try
            {
                this.BlogManager.Search();
            }
            catch
            {

            }
        }

        public void Post()
        {
            try
            {
                this.BlogManager.Post(this.WPParameter!);
            }
            catch
            {

            }
        }
    }
}
