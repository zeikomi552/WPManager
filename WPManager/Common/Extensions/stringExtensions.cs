using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPManager.Common.Extensions
{
    public static class stringExtensions
    {
        public static string EmptyToText(this string str, string replace_text)
        {
            if (string.IsNullOrEmpty(str))
            {
                return replace_text;
            }
            else
            {
                return str;
            }
        }

        public static string CutText(this string str, int length)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > length)
            {
                return str.Substring(0, length) + "...";
            }
            else
            {
                return str;
            }

        }

        /// <summary>
        /// Htmlコードのエスケープ処理
        /// </summary>
        /// <param name="str">文字列</param>
        /// <returns>エスケープ文字の変換結果</returns>
        public static string HtmlCodeEscape(this string str)
        {
            return str
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("'", "&#39;")
                .Replace("\"", "&quot;");
        }
    }
}
