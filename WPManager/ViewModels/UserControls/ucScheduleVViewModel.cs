using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPManager.Models;
using WPManager.Models.Civitai;
using WPManager.Models.Civitai.Enums;
using WPManager.Models.GitHub;
using WPManager.Models.Schedule;

namespace WPManager.ViewModels.UserControls
{
    public class ucScheduleVViewModel : BindableBase
    {
        public ucScheduleVViewModel(IGlobalConfigM gConfig)
        {
            if (gConfig != null && gConfig.ScheduleConfig != null)
            {
                this.ScheduleConf = gConfig.ScheduleConfig;
            }

            this.Config = gConfig;
        }
        #region スケジュール設定データ
        /// <summary>
        /// スケジュール設定データ
        /// </summary>
        ScheduleConfigM _ScheduleConf = new ScheduleConfigM();
        /// <summary>
        /// スケジュール設定データ
        /// </summary>
        public ScheduleConfigM ScheduleConf
        {
            get
            {
                return _ScheduleConf;
            }
            set
            {
                if (_ScheduleConf == null || !_ScheduleConf.Equals(value))
                {
                    _ScheduleConf = value;
                    RaisePropertyChanged("ScheduleConf");
                }
            }
        }
        #endregion

        #region コンフィグ情報
        /// <summary>
        /// コンフィグ情報
        /// </summary>
        IGlobalConfigM? _Config;
        /// <summary>
        /// コンフィグ情報
        /// </summary>
        public IGlobalConfigM? Config
        {
            get
            {
                return _Config;
            }
            set
            {
                if (_Config == null || !_Config.Equals(value))
                {
                    _Config = value;
                    RaisePropertyChanged("Config");
                }
            }
        }
        #endregion



        #region 選択しているスケジュール要素
        /// <summary>
        /// 選択しているスケジュール要素
        /// </summary>
        ScheduleM? _SelectedScheduleItem = null;
        /// <summary>
        /// 選択しているスケジュール要素
        /// </summary>
        public ScheduleM? SelectedScheduleItem
        {
            get
            {
                return _SelectedScheduleItem;
            }
            set
            {
                if (_SelectedScheduleItem == null || !_SelectedScheduleItem.Equals(value))
                {
                    _SelectedScheduleItem = value;
                    RaisePropertyChanged("SelectedScheduleItem");
                }
            }
        }
        #endregion

        public async void Post()
        {
            foreach (var item in this.ScheduleConf.ScheduleItems)
            {
                if (item.Selected == true)
                {
                    switch (item.SourceType)
                    {
                        case Models.WordPress.SourceTypeEnum.CivitaiModel:
                            {
                                CivitaiBlogManagerM civitai_model = new CivitaiBlogManagerM();
                                civitai_model.SearchAndPost(item, this.Config!.WPConfig!);
                                break;
                            }
                        case Models.WordPress.SourceTypeEnum.CivitaiImage:
                            {
                                CivitaiBlogImageManagerM civitai_model = new CivitaiBlogImageManagerM();
                                civitai_model.SearchAndPost(item, this.Config!.WPConfig!);
                                break;
                            }
                        case Models.WordPress.SourceTypeEnum.GitHubRepository:
                            {
                                GitHubBlogManagerM model = new GitHubBlogManagerM();
                                model.SearchAndPost(item, this.Config);
                                break;
                            }
                    }
                }
            }
        }
    }
}
