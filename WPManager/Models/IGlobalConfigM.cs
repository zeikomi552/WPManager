using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPManager.Common.Utilites;
using WPManager.Models.Schedule;

namespace WPManager.Models
{
    public interface IGlobalConfigM
    {
        public string ConfigDir { get; }
        public WPParameterM? WPConfig { get; set; }
        public GitHubParameterM? GitHubConfig { get; set; }
        public ScheduleConfigM? ScheduleConfig { get; set; }
        public void Load();

        public void Save();
    }
}
