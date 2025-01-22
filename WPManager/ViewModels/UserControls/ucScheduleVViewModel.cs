using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPManager.Common.Utilites;
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





        #region 投稿処理
        /// <summary>
        /// 投稿処理
        /// </summary>
        public void Post()
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
                        case Models.WordPress.SourceTypeEnum.GitHubLanguage:
                            {
                                GitHubBolgLanguageManagerM model = new GitHubBolgLanguageManagerM();
                                model.SearchAndPost(item, this.Config!);
                                break;
                            }
                        case Models.WordPress.SourceTypeEnum.GitHubRepository:
                            {
                                GitHubBlogManagerM model = new GitHubBlogManagerM();
                                model.SearchAndPost(item, this.Config!);
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
            }
        }
        #endregion

        #region ファイルの保存処理
        /// <summary>
        /// ファイルの保存処理
        /// </summary>
        public void Save()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new SaveFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "テキストファイル (*.wpmconf)|*.wpmconf";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    XMLUtil.Seialize<ScheduleConfigM>(dialog.FileName, this.ScheduleConf);
                }
            }
            catch
            {

            }
        }
        #endregion

        #region ファイルのロード処理
        /// <summary>
        /// ファイルのロード処理
        /// </summary>
        public void Load()
        {
            try
            {

                // ダイアログのインスタンスを生成
                var dialog = new OpenFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "テキストファイル (*.wpmconf)|*.wpmconf";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    this.ScheduleConf = XMLUtil.Deserialize<ScheduleConfigM>(dialog.FileName);
                }
            }
            catch
            {

            }
        }
        #endregion

        #region コピー処理
        /// <summary>
        /// コピー処理
        /// </summary>
        public void Copy()
        {
            try
            {
                if (ScheduleConf.SelectedScheduleItem != null)
                {
                    var copy = ScheduleConf.SelectedScheduleItem!.ShallowCopy();
                    this.ScheduleConf.ScheduleItems.Add(copy);
                }
            }
            catch
            {

            }
        }
        #endregion

        #region ↑へ移動
        /// <summary>
        /// ↑へ移動
        /// </summary>
        public void MoveUp()
        {
            try
            {
                if (this.ScheduleConf.SelectedScheduleItem != null)
                {
                    this.ScheduleConf.MoveUP();
                }

            }
            catch
            {

            }
        }
        #endregion

        #region ↓へ移動
        /// <summary>
        /// ↓へ移動
        /// </summary>
        public void MoveDown()
        {
            try
            {
                if (this.ScheduleConf.SelectedScheduleItem != null)
                {
                    this.ScheduleConf.MoveDown();
                }
            }
            catch
            {

            }
        }
        #endregion
    }
}
