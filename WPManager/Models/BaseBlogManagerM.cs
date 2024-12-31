using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPManager.Models
{
    public class BaseBlogManagerM : BindableBase
    {

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


        #region 記事のポスト処理
        /// <summary>
        /// 記事のポスト処理
        /// </summary>
        /// <param name="wpParam"></param>
        public void Post(IWPParameterM wpParam)
        {
            Task.Run(() =>
            {
                wpParam.CreateOrUpdatePost(this.Article).Wait();
            });
        }
        #endregion
    }
}
