using DryIoc.ImTools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WPManager.Models.Schedule
{
    public class ScheduleConfigM : BindableBase
    {
        #region スケジュール設定用オブジェクト
        /// <summary>
        /// スケジュール設定用オブジェクト
        /// </summary>
        ObservableCollection<ScheduleM> _ScheduleItems = new ObservableCollection<ScheduleM>();
        /// <summary>
        /// スケジュール設定用オブジェクト
        /// </summary>
        public ObservableCollection<ScheduleM> ScheduleItems
        {
            get
            {
                return _ScheduleItems;
            }
            set
            {
                if (_ScheduleItems == null || !_ScheduleItems.Equals(value))
                {
                    _ScheduleItems = value;
                    RaisePropertyChanged("ScheduleItems");
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
        [XmlIgnore]
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

        #region 1つ上の要素と入れ替える
        /// <summary>
        /// 1つ上の要素と入れ替える
        /// </summary>
        public void MoveUP()
        {
            if (this.SelectedScheduleItem != null)
            {
                int index = this.ScheduleItems.IndexOf(this.SelectedScheduleItem);

                if (index > 0)
                {
                    // 指定した位置の要素を取り出す
                    var elem = this.ScheduleItems.ElementAt(index);
                    // 指定した位置の要素を削除する
                    this.ScheduleItems.RemoveAt(index);
                    // 一つ上の要素に挿入する
                    this.ScheduleItems.Insert(index - 1, elem);
                    // 選択要素をセット
                    this.SelectedScheduleItem = elem;
                }
            }
        }
        #endregion


        #region 一つ下の要素と入れ替える
        /// <summary>
        /// 一つ下の要素と入れ替える
        /// </summary>
        public void MoveDown()
        {
            if (this.SelectedScheduleItem != null)
            {
                int index = this.ScheduleItems.IndexOf(this.SelectedScheduleItem);

                if (index < this.ScheduleItems.Count - 1)
                {
                    // 指定した位置の要素を取り出す
                    var elem = this.ScheduleItems.ElementAt(index);
                    // 指定した位置の要素を削除する
                    this.ScheduleItems.RemoveAt(index);
                    // 一つ上の要素に挿入する
                    this.ScheduleItems.Insert(index + 1, elem);
                    // 選択要素をセット
                    this.SelectedScheduleItem = elem;
                }
            }
        }
        #endregion


        #region 選択行を削除する処理
        /// <summary>
        /// 選択行を削除する処理
        /// </summary>
        public void SelectedItemDelete()
        {
            try
            {
                // 選択行を削除
                if (this.SelectedScheduleItem != null)
                {
                    var tmp = (from x in this.ScheduleItems
                               where x.Equals(this.SelectedScheduleItem)
                               select x).First();
                    this.ScheduleItems.Remove(tmp);
                }

            }
            catch// (Exception ex)
            {
                //ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion


        #region 最初の要素を選択する
        /// <summary>
        /// 最初の要素を選択する
        /// </summary>
        public void SelectedFirst()
        {
            if (this.ScheduleItems != null && this.ScheduleItems.Count > 0)
            {
                this.SelectedScheduleItem = this.ScheduleItems.ElementAt(0);
            }
        }
        #endregion

        #region 最後の要素を選択する
        /// <summary>
        /// 最後の要素を選択する
        /// </summary>
        public void SelectedLast()
        {
            if (this.ScheduleItems != null && this.ScheduleItems.Count > 0)
            {
                this.SelectedScheduleItem = this.ScheduleItems.Last();
            }
        }
        #endregion
    }
}
