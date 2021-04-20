using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.MXC.API
{
    public class ResetClientWrapper
    {
        private string secretkey;
        private string accessKey;
        private HttpClient client;
        private static string host = "https://www.mxc.com";

        public ResetClientWrapper(string secretkey,string accessKey)
        {
            this.secretkey = secretkey;
            this.accessKey = accessKey;
            client = new HttpClient();
           // client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla / 5.0(Windows NT 10.0; WOW64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 72.0.3626.96 Safari / 537.36");
            ping();
        }

        public T Get<T>(string url, Dictionary<string, string> param) where T : class
        {
            return call<T>("GET", url, param, false);
        }
        private T call<T>(string method, string url,Dictionary<string,string> param, bool needSign) where T : class
        {
            param.Add("api_key", this.accessKey);
            if (needSign)
            {
                TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
                param.Add("req_time", (int)t.TotalSeconds + "");
                param.Add("recv_window", "60");
                param.Add("sign", Utils.CreateSignature(method,url,param,this.secretkey));           
            }

            if(method == "GET")
            {
                Utils.GetRequestParamString(param);
                var request = client.GetAsync(host + url + "?" + Utils.GetRequestParamString(param));
                request.Wait();
                var responseContent = request.Result.Content.ReadAsStringAsync();
                responseContent.Wait();
                var result = responseContent.Result;
                var data = JsonConvert.DeserializeObject<T>(responseContent.Result);
                return data;
            }

            return null;
        }

        private void ping()
        {
            // implement ping method
        }


        public void Dispose()
        {
            client.Dispose();
        }
    }
}
