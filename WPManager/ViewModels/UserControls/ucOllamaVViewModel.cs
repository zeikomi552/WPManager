using Ollapi.api;
using Ollapi.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WPManager.Common.Utilites;

namespace WPManager.ViewModels.UserControls
{
    public class ucOllamaVViewModel : BindableBase
    {
        #region 送信用メッセージ
        /// <summary>
        /// 送信用メッセージ
        /// </summary>
        string _PostMessage = string.Empty;
        /// <summary>
        /// 送信用メッセージ
        /// </summary>
        public string PostMessage
        {
            get
            {
                return _PostMessage;
            }
            set
            {
                if (_PostMessage == null || !_PostMessage.Equals(value))
                {
                    _PostMessage = value;
                    RaisePropertyChanged("PostMessage");
                }
            }
        }
        #endregion

        #region メッセージ履歴
        /// <summary>
        /// メッセージ履歴
        /// </summary>
        ObservableCollection<IOllapiMessage> _MessageHistory = new ObservableCollection<IOllapiMessage>();
        /// <summary>
        /// メッセージ履歴
        /// </summary>
        public ObservableCollection<IOllapiMessage> MessageHistory
        {
            get
            {
                return _MessageHistory;
            }
            set
            {
                if (_MessageHistory == null || !_MessageHistory.Equals(value))
                {
                    _MessageHistory = value;
                    RaisePropertyChanged("MessageHistory");
                }
            }
        }
        #endregion


        #region 返答
        /// <summary>
        /// 返答
        /// </summary>
        string _ReturnMessage = string.Empty;
        /// <summary>
        /// 返答
        /// </summary>
        public string ReturnMessage
        {
            get
            {
                return _ReturnMessage;
            }
            set
            {
                if (_ReturnMessage == null || !_ReturnMessage.Equals(value))
                {
                    _ReturnMessage = value;
                    RaisePropertyChanged("ReturnMessage");
                }
            }
        }
        #endregion


        public async void Post()
        {
            try
            {
                this.MessageHistory.Add(
                    new OllapiMessage()
                    {
                        Role = "user",
                        Content = PostMessage
                    }
                    );

                OllapiChatRequest test = new OllapiChatRequest("localhost", 11434, "example");
                test.Open();
                var ret = await test.Request(MessageHistory.ToList());

                int retry = 0;
                while (retry < 10)
                {
                    try
                    {
                        var tmp = JSONUtil.DeserializeFromText<OllapiChatResponse>(ret);

                        if (tmp.Message != null)
                        {
                            this.MessageHistory.Add(tmp.Message);
                            break;
                        }
                    }
                    catch
                    {
                        retry++;
                    }
                }

                test.Close();

                this.PostMessage = string.Empty;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void Clear()
        {
            try
            {
                this.MessageHistory.Clear();
                this.ReturnMessage = string.Empty;

            }
            catch
            {
            }
        }

        //public string ImageToBase64(Image image, ImageFormat format)
        //{
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        // 画像をストリームに保存
        //        image.Save(ms, format);
        //        byte[] imageBytes = ms.ToArray();

        //        // バイト配列をBase64文字列に変換
        //        string base64String = Convert.ToBase64String(imageBytes);
        //        return base64String;
        //    }
        //}
    }
}
