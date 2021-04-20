using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace NodeBlock.Plugin.Exchange.Nodes.MXC.API
{
    class Utils
    {
        public static string CreateSignature(String method, String uri, Dictionary<string, string> param, string secretKey)
        {
            if (param.Count == 0)
            {
                return "";
            }

            var sb = new StringBuilder(1024);
            sb.Append(method.ToUpper()).Append('\n')
              .Append(uri).Append('\n');
            SortedDictionary<string, string> map = new SortedDictionary<string, string>(param);

            foreach (var entry in map)
            {
                string key = entry.Key;
                string value = string.IsNullOrEmpty(entry.Value) ? "" : entry.Value;
                sb.Append(key).Append("=").Append(UrlEncode(value)).Append("&");
            }

            sb.Length--;

            return ActualSignature(sb.ToString(), secretKey);
        }

        public static string GetRequestParamString(Dictionary<string,string> param)
        {
            if(param.Count == 0)
            {
                return "";
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

        public static string ActualSignature(string inputStr,string key)
        {
            var hmac = new HMACSHA256(System.Text.Encoding.UTF8.GetBytes(key));
            var hesh  = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(inputStr));
            return ToHexString(hesh);
        }

        public static string ToHexString(byte[] bytes) 
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }
    }
}
