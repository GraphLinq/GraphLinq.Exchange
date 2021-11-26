using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.LiveCoinWatch.API
{
    public class OverviewResponse
    {
        [JsonProperty("liquidity")]
        public double Liquidity { get; set; }
    
        [JsonProperty("btcDominance")]
        public double btcDominance { get; set; }

        [JsonProperty("volume")]
        public double Volume { get; set; }

        [JsonProperty("cap")]
        public double Cap { get; set; }
    }
}
