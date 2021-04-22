using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Linq;
namespace NodeBlock.Plugin.Exchange.Nodes.MXC.API
{
    class Utils
    {
        public static string CreateSignature(String method, String uri, Dictionary<string, string> param, string secretKey)
        {
            if (param.Count == 0)
            {
                return string.Empty;
            }

            var sb = new StringBuilder(1024);
            sb.Append(method.ToUpper()).Append("\n")
              .Append(uri).Append("\n");
            SortedDictionary<string, string> map = new SortedDictionary<string, string>(param);
            
            foreach (var entry in map)
            {
                string key = entry.Key;
                string value = string.IsNullOrEmpty(entry.Value) ? "" : entry.Value;
                sb.Append(key).Append("=").Append(UrlEncode(value)).Append("&");
            }

            sb.Length--;
            var str = sb.ToString();
            return ActualSignature(str, secretKey);
        }

        public static string GetRequestParamString(Dictionary<string,string> param)
        {
            if(param.Count == 0)
            {
                return string.Empty;
            }

            var sb = new StringBuilder(1024);
            SortedDictionary<string, string> map = new SortedDictionary<string, string>(param);

            foreach(var entry in map)
            {
                string key = entry.Key;
                string value = string.IsNullOrEmpty(entry.Value) ? "" : entry.Value;
                sb.Append(key).Append("=").Append(UrlEncode(value)).Append("&");
            }

            sb.Length--;

            return sb.ToString();
        }

        public static string UrlEncode(string s)
        {
            return HttpUtility.UrlEncode(s, Encoding.UTF8);
        }

        public static string Sign(SignVo signVo)
        {
            string str = signVo.AcessKey + signVo.ReqTime + signVo.RequestParam;
            return ActualSignature(str, signVo.SecretKey);
        }

        private static byte[] StringEncode(string text)
        {
            var encoding = new UTF8Encoding();
            return encoding.GetBytes(text);
        }

        private static string HashEncode(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        public static string ActualSignature(string inputStr, string key)
        {
            var hmac = new HMACSHA256(StringEncode(key));
            var hesh = hmac.ComputeHash(StringEncode(inputStr));
            return HashEncode(hesh);
        }
    }
}
