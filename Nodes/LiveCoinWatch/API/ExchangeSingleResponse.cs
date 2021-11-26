using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.LiveCoinWatch.API
{
    public class ExchangeSingleResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("png128")]
        public string Png128 { get; set; }

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

        [JsonProperty("volume")]
        public double Volume { get; set; }

        [JsonProperty("bidTotal")]
        public double BidTotal { get; set; }

        [JsonProperty("askTotal")]
        public double AskTotal { get; set; }

        [JsonProperty("depth")]
        public double Depth { get; set; }

        [JsonProperty("visitors")]
        public int Visitors { get; set; }

        [JsonProperty("volumePerVisitor")]
        public double volumePerVisitor { get; set; }
    }
}
