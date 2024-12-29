using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WPManager.Models.Civitai
{
    public class CivitaiRequest
    {
        #region 接続用クライアントの作成
        /// <summary>
        /// 接続用クライアントの作成
        /// </summary>
        /// <param name="url">パラメータ</param>
        /// <returns>Task</returns>
        public async Task<string> Request(string url)
        {
            using (var client = new HttpClient())
            {
                // 上から来たクエリをそのまま実行
                var response = await client.GetAsync(url);

                // レスポンスを返却
                return await response.Content.ReadAsStringAsync();
            }
        }
        #endregion
    }
}
