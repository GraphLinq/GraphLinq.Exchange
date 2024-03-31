using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.LiveCoinWatch.API
{

    public partial class CoinSingleResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

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
        public double CirculatingSupply { get; set; }

        [JsonProperty("maxSupply")]
        public double MaxSupply { get; set; }

        [JsonProperty("totalSupply")]
        public double TotalSupply { get; set; }

        [JsonProperty("rate")]
        public double Rate { get; set; }

        [JsonProperty("volume")]
        public double Volume { get; set; }

        [JsonProperty("cap")]
        public double Cap { get; set; }

        [JsonProperty("delta")]
        public JsonArrayAttribute Deltas { get; set; }
    }

    public partial class Deltas
    {
        [JsonProperty("hour")]
        public double? Hour { get; set; }

        [JsonProperty("day")]
        public double? Day { get; set; }

        [JsonProperty("week")]
        public double? Week { get; set; }

        [JsonProperty("month")]
        public double? Month { get; set; }

        [JsonProperty("quarter")]
        public double? Quarter { get; set; }

        [JsonProperty("year")]
        public double? Year { get; set; }

    }
}
