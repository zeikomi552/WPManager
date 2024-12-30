using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPManager.Models.Civitai.Models;

namespace WPManager.Models
{
    public class WPDataObjectM
    {
        /// <summary>
        /// ポストID
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// タイトル
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 説明
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 抜粋
        /// </summary>
        public string Excerpt {  get; set; } = string.Empty;

        /// <summary>
        /// コンテンツ
        /// </summary>
        public string Content {  get; set; } = string.Empty;

        /// <summary>
        /// スラッグ
        /// </summary>
        public string Slug { get; set; } = string.Empty;
        /// <summary>
        /// カテゴリーリスト
        /// </summary>
        public List<int> Categories { get; set; } = new List<int>();
        /// <summary>
        /// タグリスト
        /// </summary>
        public List<int> Tags { get; set; } = new List<int>();


        public void SetRequest(CvsModelM model)
        {

        }


    }
}
