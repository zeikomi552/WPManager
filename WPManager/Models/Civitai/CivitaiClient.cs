using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPManager.Common.Utilites;
using WPManager.Models.Civitai.Models;

namespace WPManager.Models.Civitai
{
    public class CivitaiClient : BindableBase
    {
        public string EndPoint { get;} = string.Empty;

        public CivitaiClient(string endpoint)
        {
            this.EndPoint = endpoint;
        }

        public async Task<CvsModelM> ModelSearch(string query)
        {
            string request = string.Empty;

            // エンドポイント + パラメータ
            string url = CvsModelM.Endpoint + query;

            CivitaiRequest tmp = new CivitaiRequest();

            // 実行してJSON形式をデシリアライズ
            var request_tmp = await tmp.Request(url);

            // Jsonファイルを展開して返却
            return JSONUtil.DeserializeFromText<CvsModelM>(request_tmp);
        }
    }
}
