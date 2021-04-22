using Newtonsoft.Json;
using NodeBlock.Plugin.Exchange.Nodes.MXC.Entities;
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
            ping();
        }

        private void ping()
        {
           var result =  Get<PingEntity>("/open/api/v2/common/ping", new Dictionary<string, string>());
            if(result.code == 0)
            {
                throw new Exception("Can't reach url");
            }
        }

        public T Get<T>(string url, Dictionary<string, string> param,bool sign = false) where T : class
        {
            return call<T>("GET", url, null,param, sign);
        }

        public T Post<T>(string url, Dictionary<string, string> param, bool sign = false) where T : class
        {
            return call<T>("POST", url, param, new Dictionary<string, string>(), sign);
        }

        private T call<T>(string method, string url, Dictionary<string, string> objects, Dictionary<string,string> param, bool needSign) where T : class
        {
            param.Add("api_key", this.accessKey);
            if (needSign)
            {
                TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
                param.Add("req_time", (int)t.TotalSeconds + "");
                param.Add("sign", Utils.CreateSignature(method,url,param,this.secretkey));           
            }

            if(method == "GET")
            {
                var request_url = host + url + "?" + Utils.GetRequestParamString(param);
                var request = client.GetAsync(request_url);
                request.Wait();
                var responseContent = request.Result.Content.ReadAsStringAsync();
                responseContent.Wait();
                var result = responseContent.Result;
                var data = JsonConvert.DeserializeObject<T>(responseContent.Result);

                dynamic code = data;
                if (code.code != 200)
                {
                    throw new Exception("Failed params request ! ");
                }
                return data;
            }
            else if (method == "POST")
            {
                var request_url = host + url + "?" + Utils.GetRequestParamString(param);
                var json = JsonConvert.SerializeObject(objects);
                var request = client.PostAsync(request_url, new StringContent(json, Encoding.UTF8, "application/json"));
                request.Wait();
                var responseContent = request.Result.Content.ReadAsStringAsync();
                responseContent.Wait();
                var result = responseContent.Result;
                var data = JsonConvert.DeserializeObject<T>(responseContent.Result);
                dynamic code = data;

                if (code.code != 200)
                {
                    throw new Exception("Failed params request ! ");
                }

                return data;

            }

            return null;
        }

       


        public void Dispose()
        {
            client.Dispose();
        }
    }
}
