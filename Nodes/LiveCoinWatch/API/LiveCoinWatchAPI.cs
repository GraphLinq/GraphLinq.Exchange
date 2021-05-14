using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.LiveCoinWatch.API
{
    public class LiveCoinWatchAPI
    {
        public string APIKey { get; }
        private HttpClient client = new HttpClient();
        private string baseUrl = "https://api.livecoinwatch.com";

        public LiveCoinWatchAPI(string apiKey)
        {
            this.APIKey = apiKey;
            this.client.DefaultRequestHeaders.Add("x-api-key", this.APIKey);
        }

        public async Task<CoinSingleResponse> FetchCoinSingle(string currency, string ticker)
        {
            var json = JsonConvert.SerializeObject(new
            {
                currency = currency,
	            code = ticker,
                meta = true
            });
            var request = await client.PostAsync(baseUrl + "/coins/single", new StringContent(json, Encoding.UTF8, "application/json"));
            var responseContent = await request.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<CoinSingleResponse>(responseContent);
            return data;
        }
    }
}
