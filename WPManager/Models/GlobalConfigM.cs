using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WPManager.Common.Utilites;

namespace WPManager.Models
{
    public class GlobalConfigM : IGlobalConfigM
    {
        #region ディレクトリ名
        /// <summary>
        /// ディレクトリ名
        /// </summary>
        [XmlIgnore]
        public string ConfigDir
        {
            get { return "Config"; }
        }
        #endregion

        /// <summary>
        /// Wordpress用Configファイルオブジェクト
        /// </summary>
        public WPParameterM? WPConfig { get; set; }

        public void Load()
        {
            this.WPConfig = LoadConfig<WPParameterM>(this.ConfigDir, "WPConfig.conf");
        }

        public void Save()
        {
            SaveConfig<WPParameterM>(this.ConfigDir, "WPConfig.conf", this.WPConfig!);
        }


        #region Wordpress用ファイルの読み込み
        /// <summary>
        /// Wordpress用Configファイルの読み込み
        /// </summary>
        public T? LoadConfig<T>(string dir, string filename) where T : new()
        {
            try
            {
                var tmp = new ConfigManager<T>(dir, filename, new T());

                // ファイルの存在確認
                if (!File.Exists(tmp.ConfigFile))
                {
                    tmp.SaveXML(); // XMLのセーブ
                }
                else
                {
                    tmp.LoadXML(); // XMLのロード
                }
                return tmp.Item;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Wordpress用ファイルの読み込み
        /// <summary>
        /// Wordpress用Configファイルの読み込み
        /// </summary>
        public void SaveConfig<T>(string dir, string filename, T value) where T : new()
        {
            try
            {
                var tmp = new ConfigManager<T>(dir, filename, new T());

                tmp.Item = value;

                tmp.SaveXML(); // XMLのセーブ
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
