using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
