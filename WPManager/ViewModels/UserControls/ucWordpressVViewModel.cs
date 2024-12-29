using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WordPressPCL.Models;
using WordPressPCL;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata;
using WordPressPCL.Client;
using WPManager.Models;

namespace WPManager.ViewModels.UserControls
{
    public class ucWordpressVViewModel : BindableBase
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

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="wpParameter"></param>
        public ucWordpressVViewModel(IWPParameterM wpParameter)
        {
            this.WPParameter = wpParameter;
        }

        #region ブログ記事一覧
        /// <summary>
        /// ブログ記事一覧
        /// </summary>
        ObservableCollection<Post> _BlogArticle = new ObservableCollection<Post>();
        /// <summary>
        /// ブログ記事一覧
        /// </summary>
        public ObservableCollection<Post> BlogArticle
        {
            get
            {
                return _BlogArticle;
            }
            set
            {
                if (_BlogArticle == null || !_BlogArticle.Equals(value))
                {
                    _BlogArticle = value;
                    RaisePropertyChanged("BlogArticle");
                }
            }
        }
        #endregion

        #region 選択記事
        /// <summary>
        /// 選択記事
        /// </summary>
        Post _SelectedBlogArticle = new Post();
        /// <summary>
        /// 選択記事
        /// </summary>
        public Post SelectedBlogArticle
        {
            get
            {
                return _SelectedBlogArticle;
            }
            set
            {
                if (_SelectedBlogArticle == null || !_SelectedBlogArticle.Equals(value))
                {
                    _SelectedBlogArticle = value;
                    RaisePropertyChanged("SelectedBlogArticle");
                }
            }
        }
        #endregion



        public void GetAllPost()
        {
            Task.Run(async () =>
            {
                this.BlogArticle = await this.WPParameter!.GetPost();
            });
        }

        public void Post()
        {
            Task.Run(() =>
            {
                this.WPParameter!.CreateOrUpdatePost(0, "test", "test-description", "string-excerpt", "test-content", "test-aaa").Wait();
            });
            
        }

    }
}
