using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WPManager.Models.Civitai;
using WPManager.Models.WordPress;

namespace WPManager.Models
{
    public class BaseBlogManagerM : BindableBase
    {
        #region ブログタイプ
        /// <summary>
        /// ブログタイプ
        /// </summary>
        PostOrPage _PostOrPage = PostOrPage.Post;
        /// <summary>
        /// ブログタイプ
        /// </summary>
        public PostOrPage PostOrPage
        {
            get
            {
                return _PostOrPage;
            }
            set
            {
                if (!_PostOrPage.Equals(value))
                {
                    _PostOrPage = value;
                    RaisePropertyChanged("PostOrPage");
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


        #region 記事のポスト処理
        /// <summary>
        /// 記事のポスト処理
        /// </summary>
        /// <param name="wpParam"></param>
        public void Post(IWPParameterM wpParam)
        {
            Task.Run(() =>
            {
                if (this.PostOrPage == PostOrPage.Post)
                {
                    wpParam.CreateOrUpdatePost(this.Article).Wait();
                }
                else
                {
                    wpParam.CreateOrUpdatePages(this.Article).Wait();
                }
            });
        }
        #endregion
    }
}
