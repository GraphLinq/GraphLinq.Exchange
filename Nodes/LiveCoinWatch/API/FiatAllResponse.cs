using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.LiveCoinWatch.API
{
    public class FiatAllResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("countries")]
        public Array Countries { get; set; }

        [JsonProperty("flag")]
        public string Flag { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }
    }
}
