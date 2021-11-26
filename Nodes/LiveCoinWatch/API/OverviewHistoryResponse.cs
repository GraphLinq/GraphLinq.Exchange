using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.LiveCoinWatch.API
{
    public class OverviewHistoryResponse
    {
        [JsonProperty("date")]
        public int Date { get; set; }
        
        [JsonProperty("liquidity")]
        public int Liquidity { get; set; }
    
        [JsonProperty("btcDominance")]
        public int btcDominance { get; set; }

        [JsonProperty("volume")]
        public int Volume { get; set; }

        [JsonProperty("cap")]
        public int Cap { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}
