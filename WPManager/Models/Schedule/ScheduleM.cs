using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;
using WPManager.Models.Civitai.Enums;
using WPManager.Models.WordPress;

namespace WPManager.Models.Schedule
{
    public class ScheduleM : BindableBase
    {
        #region 対象
        /// <summary>
        /// 対象
        /// </summary>
        bool _Selected = false;
        /// <summary>
        /// 対象
        /// </summary>
        [XmlIgnore]
        public bool Selected
        {
            get
            {
                return _Selected;
            }
            set
            {
                if (!_Selected.Equals(value))
                {
                    _Selected = value;
                    RaisePropertyChanged("Selected");
                }
            }
        }
        #endregion

        #region タイトル
        /// <summary>
        /// タイトル
        /// </summary>
        string _Title = string.Empty;
        /// <summary>
        /// タイトル
        /// </summary>
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                if (_Title == null || !_Title.Equals(value))
                {
                    _Title = value;
                    RaisePropertyChanged("Title");
                }
            }
        }
        #endregion

        #region 記事Id
        /// <summary>
        /// 記事Id
        /// </summary>
        int _ArticleId = 0;
        /// <summary>
        /// 記事Id
        /// </summary>
        public int ArticleId
        {
            get
            {
                return _ArticleId;
            }
            set
            {
                if (!_ArticleId.Equals(value))
                {
                    _ArticleId = value;
                    RaisePropertyChanged("ArticleId");
                }
            }
        }
        #endregion

        #region Post(投稿記事) or Page(固定ページを示す列挙体)
        /// <summary>
        /// Post(投稿記事) or Page(固定ページを示す列挙体)
        /// </summary>
        PostOrPage _PostPageType = new PostOrPage();
        /// <summary>
        /// Post(投稿記事) or Page(固定ページを示す列挙体)
        /// </summary>
        public PostOrPage PostPageType
        {
            get
            {
                return _PostPageType;
            }
            set
            {
                if (!_PostPageType.Equals(value))
                {
                    _PostPageType = value;
                    RaisePropertyChanged("PostPageType");
                }
            }
        }
        #endregion

        #region スラッグ
        /// <summary>
        /// スラッグ
        /// </summary>
        string _Slug = string.Empty;
        /// <summary>
        /// スラッグ
        /// </summary>
        public string Slug
        {
            get
            {
                return _Slug;
            }
            set
            {
                if (_Slug == null || !_Slug.Equals(value))
                {
                    _Slug = value;
                    RaisePropertyChanged("Slug");
                }
            }
        }
        #endregion

        #region モデルタイプ(Civitai限定)
        /// <summary>
        /// モデルタイプ(Civitai限定)
        /// </summary>
        ModelTypeEnum _ModelType = ModelTypeEnum.Checkpoint;
        /// <summary>
        /// モデルタイプ(Civitai限定)
        /// </summary>
        public ModelTypeEnum ModelType
        {
            get
            {
                return _ModelType;
            }
            set
            {
                if (!_ModelType.Equals(value))
                {
                    _ModelType = value;
                    RaisePropertyChanged("ModelType");
                }
            }
        }
        #endregion

        #region 元データタイプ(Civitai(Model), GitHub(Language)等)
        /// <summary>
        /// 元データタイプ(Civitai(Model), GitHub(Language)等)
        /// </summary>
        SourceTypeEnum _SourceType = new SourceTypeEnum();
        /// <summary>
        /// 元データタイプ(Civitai(Model), GitHub(Language)等)
        /// </summary>
        public SourceTypeEnum SourceType
        {
            get
            {
                return _SourceType;
            }
            set
            {
                if (!_SourceType.Equals(value))
                {
                    _SourceType = value;
                    RaisePropertyChanged("SourceType");
                }
            }
        }
        #endregion

        #region 記事タイプ
        /// <summary>
        /// 記事タイプ
        /// </summary>
        int _ArticleType = 1;
        /// <summary>
        /// 記事タイプ
        /// </summary>
        public int ArticleType
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

        #region 検索開始日(GitHubOnly)
        /// <summary>
        /// 検索開始日(GitHubOnly)
        /// </summary>
        DateTime _SearchFrom = DateTime.MinValue;
        /// <summary>
        /// 検索開始日(GitHubOnly)
        /// </summary>
        public DateTime SearchFrom
        {
            get
            {
                return _SearchFrom;
            }
            set
            {
                if (!_SearchFrom.Equals(value))
                {
                    _SearchFrom = value;
                    RaisePropertyChanged("SearchFrom");
                }
            }
        }
        #endregion

        #region 検索終了日(GitHubOnly)
        /// <summary>
        /// 検索終了日(GitHubOnly)
        /// </summary>
        DateTime _SearchTo = DateTime.MinValue;
        /// <summary>
        /// 検索終了日(GitHubOnly)
        /// </summary>
        public DateTime SearchTo
        {
            get
            {
                return _SearchTo;
            }
            set
            {
                if (!_SearchTo.Equals(value))
                {
                    _SearchTo = value;
                    RaisePropertyChanged("SearchTo");
                }
            }
        }
        #endregion

        #region 検索対象範囲(CivitaiOnly)
        /// <summary>
        /// 検索対象範囲(CivitaiOnly)
        /// </summary>
        ModelPeriodEnum _CivitaiPeriod = new ModelPeriodEnum();
        /// <summary>
        /// 検索対象範囲(CivitaiOnly)
        /// </summary>
        public ModelPeriodEnum CivitaiPeriod
        {
            get
            {
                return _CivitaiPeriod;
            }
            set
            {
                if (!_CivitaiPeriod.Equals(value))
                {
                    _CivitaiPeriod = value;
                    RaisePropertyChanged("CivitaiPeriod");
                }
            }
        }
        #endregion

        #region 検索言語
        /// <summary>
        /// 検索言語
        /// </summary>
        Language? _GitHubLanguage = null;
        /// <summary>
        /// 検索言語
        /// </summary>
        public Language? GitHubLanguage
        {
            get
            {
                return _GitHubLanguage;
            }
            set
            {
                if (_GitHubLanguage == null || !_GitHubLanguage.Equals(value))
                {
                    _GitHubLanguage = value;
                    RaisePropertyChanged("GitHubLanguage");
                }
            }
        }
        #endregion



    }
}
