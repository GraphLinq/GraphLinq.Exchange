using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.LiveCoinWatch.API
{
    public class ExchangeListResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("png64")]
        public string Png64 { get; set; }

        [JsonProperty("png128")]
        public string Png128 { get; set; }

        [JsonProperty("centralized")]
        public bool Centralized { get; set; }

        [JsonProperty("markets")]
        public int Markets { get; set; }

        [JsonProperty("volume")]
        public int Volume { get; set; }

        [JsonProperty("bidTotal")]
        public double BidTotal { get; set; }

        [JsonProperty("askTotal")]
        public int AskTotal { get; set; }

        [JsonProperty("depth")]
        public int Depth { get; set; }

        [JsonProperty("visitors")]
        public int Visitors { get; set; }

        [JsonProperty("volumePerVisitor")]
        public double volumePerVisitor { get; set; }

    }
}
