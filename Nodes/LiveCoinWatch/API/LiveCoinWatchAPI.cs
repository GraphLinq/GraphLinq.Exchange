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

        public async Task<CoinSingleResponse> FetchCoinSingle(string symbol, string currency)
        {
            var json = JsonConvert.SerializeObject(new
            {
                code = symbol,
                currency = currency,
                meta = true
            });

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var request = await client.PostAsync(baseUrl + "/coins/single", new StringContent(json, Encoding.UTF8, "application/json"));
            var responseContent = await request.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<CoinSingleResponse>(responseContent, settings);
            return data;
        }

        public async Task<CoinSingleHistoryResponse> FetchCoinSingleHistory(string currency, string code, object start, object end)
        {
            var json = JsonConvert.SerializeObject(new
            {
                currency = currency,
                code = code,
                start = start,
                end = end
            });
            var request = await client.PostAsync(baseUrl + "/coins/single/history", new StringContent(json, Encoding.UTF8, "application/json"));
            var responseContent = await request.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<CoinSingleHistoryResponse>(responseContent);
            return data;
        }

        public async Task<CoinListResponse> FetchListCoin(string currency, string sort, string order, object offset, object limit)
        {
            var json = JsonConvert.SerializeObject(new
            {
                currency = currency,
	            sort = sort,
                order = order,
                offset = offset,
                limit = limit,
                meta = true
            });
            var request = await client.PostAsync(baseUrl + "/coins/list", new StringContent(json, Encoding.UTF8, "application/json"));
            var responseContent = await request.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<CoinListResponse>(responseContent);
            return data;
        }

        public async Task<FiatAllResponse> FetchAllFiat()
        {
            var json = JsonConvert.SerializeObject(new
            {

            });
            var request = await client.PostAsync(baseUrl + "/fiats/all", new StringContent(json, Encoding.UTF8, "application/json"));
            var responseContent = await request.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<FiatAllResponse>(responseContent);
            return data;
        }

        public async Task<OverviewResponse> FetchOverview(string currency)
        {
            var json = JsonConvert.SerializeObject(new
            {
                currency = currency,
                meta = true
            });
            var request = await client.PostAsync(baseUrl + "/overview", new StringContent(json, Encoding.UTF8, "application/json"));
            var responseContent = await request.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<OverviewResponse>(responseContent);
            return data;
        }

        public async Task<OverviewHistoryResponse> FetchHistoryOverview(string currency, object start, object end)
        {
            var json = JsonConvert.SerializeObject(new
            {
                currency = currency,
	            start = start,
                end = end,
                meta = true
            });
            var request = await client.PostAsync(baseUrl + "/overview/history", new StringContent(json, Encoding.UTF8, "application/json"));
            var responseContent = await request.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<OverviewHistoryResponse>(responseContent);
            return data;
        }

        public async Task<ExchangeSingleResponse> FetchSingleExchange(string currency, string exchange)
        {
            var json = JsonConvert.SerializeObject(new
            {
                currency = currency,
	            code = exchange,
                meta = true
            });
            var request = await client.PostAsync(baseUrl + "/exchanges/single", new StringContent(json, Encoding.UTF8, "application/json"));
            var responseContent = await request.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ExchangeSingleResponse>(responseContent);
            return data;
        }

        public async Task<ExchangeListResponse> FetchListExchange(string currency, string sort, string order, object offset, object limit)
        {
            var json = JsonConvert.SerializeObject(new
            {
                currency = currency,
	            sort = sort,
                order = order,
                offset = offset,
                limit = limit,
                meta = true
            });
            var request = await client.PostAsync(baseUrl + "/", new StringContent(json, Encoding.UTF8, "application/json"));
            var responseContent = await request.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ExchangeListResponse>(responseContent);
            return data;
        }
    }
}
