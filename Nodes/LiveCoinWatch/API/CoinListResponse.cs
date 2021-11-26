using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.LiveCoinWatch.API
{
    public class CoinListResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("png32")]
        public string Png32 { get; set; }

        [JsonProperty("png64")]
        public string Png64 { get; set; }

        [JsonProperty("webp32")]
        public string Webp32 { get; set; }

        [JsonProperty("webp64")]
        public string Webp64 { get; set; }

        [JsonProperty("exchanges")]
        public int Exchanges { get; set; }

        [JsonProperty("markets")]
        public int Markets { get; set; }

        [JsonProperty("pairs")]
        public int Pairs { get; set; }

        [JsonProperty("allTimeHighUSD")]
        public double AllTimeHighUSD { get; set; }

        [JsonProperty("circulatingSupply")]
        public int CirculatingSupply { get; set; }

        [JsonProperty("totalSupply")]
        public int TotalSupply { get; set; }

        [JsonProperty("maxSupply")]
        public int MaxSupply { get; set; }

        [JsonProperty("rate")]
        public double Rate { get; set; }

        [JsonProperty("volume")]
        public int Volume { get; set; }

        [JsonProperty("cap")]
        public int Cap { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }
    }
}
