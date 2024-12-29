using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordPressPCL.Models;
using WordPressPCL;
using System.Collections.ObjectModel;

namespace WPManager.Models
{
    public class WPParameterM : BindableBase, IWPParameterM
    {

        #region エンドポイント
        /// <summary>
        /// エンドポイント
        /// </summary>
        string _EndpointUri = "https://yourdomain/wp-json/";
        /// <summary>
        /// エンドポイント
        /// </summary>
        public string EndpointUri
        {
            get
            {
                return _EndpointUri;
            }
            set
            {
                if (_EndpointUri == null || !_EndpointUri.Equals(value))
                {
                    _EndpointUri = value;
                    RaisePropertyChanged("EndpointUri");
                }
            }
        }
        #endregion

        #region パスワード
        /// <summary>
        /// パスワード
        /// </summary>
        string _Password = string.Empty;
        /// <summary>
        /// パスワード
        /// </summary>
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                if (_Password == null || !_Password.Equals(value))
                {
                    _Password = value;
                    RaisePropertyChanged("Password");
                }
            }
        }
        #endregion

        #region ユーザー名
        /// <summary>
        /// ユーザー名
        /// </summary>
        string _UserName = string.Empty;
        /// <summary>
        /// ユーザー名
        /// </summary>
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                if (_UserName == null || !_UserName.Equals(value))
                {
                    _UserName = value;
                    RaisePropertyChanged("UserName");
                }
            }
        }
        #endregion


        public async Task CreateOrUpdatePost(int postid, string title, string description, string excerpt, string content, string slug)
        {
            // Get valid WordPress Client
            WordPressClient wpClient = new WordPressClient(this.EndpointUri);

            //Basic Auth
            wpClient.Auth.UseBasicAuth(this.UserName, this.Password);

            //Or, Bearer Auth using JWT tokens
            //wpClient.Auth.UseBearerAuth(JWTPlugin.JWTAuthByEnriqueChavez);
            //wpClient.Auth.RequestJWTokenAsync("username", "password");
            //var isValidToken = wpClient.Auth.IsValidJWTokenAsync;

            //Create and Set Post object
            var post = new Post
            {
                Title = new Title(title),
                Meta = new Description(description),
                Excerpt = new Excerpt(excerpt),
                Content = new Content(content),
                //slug should be in lower case with hypen(-) separator 
                Slug = slug
            };

            //// Assign one or more Categories, if any
            //if (dataObj.Categories.Count > 0)
            //{
            //    post.Categories = dataObj.Categories;
            //}

            //// Assign one or more Tags, if any
            //if (dataObj.Tags.Count > 0)
            //{
            //    post.Tags = dataObj.Tags;
            //}

            if (postid == 0)
            {
                // if you want to hide comment section
                post.CommentStatus = OpenStatus.Closed;
                // Set it to draft section if you want to review and then publish
                post.Status = Status.Draft;
                // Create and get new the post id
                //postid = wpClient.Posts.CreateAsync(post).Result.Id;
                var tmp = wpClient.Posts.CreateAsync(post).Result;

                // read Note section below - Why update the Post again?
                await wpClient.Posts.UpdateAsync(tmp);
            }
            else
            {
                // check the status of post (draft or publish) and then update
                if (IsPostDraftStatus(wpClient, postid))
                {
                    post.Status = Status.Draft;
                }

                await wpClient.Posts.UpdateAsync(post);
            }
        }



        private static bool IsPostDraftStatus(WordPressClient client, int postId)
        {
            var result = client.Posts.GetByIDAsync(postId, true, true).Result;

            if (result.Status == Status.Draft)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<ObservableCollection<Post>> GetPost()
        {
            // Initialize
            var client = new WordPressClient(this.EndpointUri);

            // Posts
            var tmp = await client.Posts.GetAllAsync();
            return new ObservableCollection<Post>(tmp);
        }
    }
}
