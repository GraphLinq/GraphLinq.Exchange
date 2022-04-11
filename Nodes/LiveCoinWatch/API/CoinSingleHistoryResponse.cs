using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.LiveCoinWatch.API
{
    public partial class CoinSingleHistoryResponse
    {
        [JsonProperty("history")]
        public List<History> History { get; set; }
    }

    public partial class History
    {
        [JsonProperty("date")]
        public long Date { get; set; }

        [JsonProperty("rate")]
        public double? Rate { get; set; }

        [JsonProperty("volume")]
        public long? Volume { get; set; }

        [JsonProperty("cap")]
        public long? Cap { get; set; }

        [JsonProperty("liquidity")]
        public long? Liquidity { get; set; }
    }
    public partial class CoinSingleHistoryResponse
    {
        public static CoinSingleHistoryResponse FromJson(string json) => JsonConvert.DeserializeObject<CoinSingleHistoryResponse>(json);
    }
}
