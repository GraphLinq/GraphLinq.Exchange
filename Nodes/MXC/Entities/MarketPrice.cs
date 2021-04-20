using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.MXC.Entities
{
    [Serializable]
    public class MarketPrice
    {
        public int code
        {
            get;set;
        }


    }

    [Serializable]
    public class DataInformation
    {
        public List<KeyValuePair<string, string>> asks
        {
            get;set;
        }
        public List<KeyValuePair<string,string>> bids
        {
            get; set;
        }

    }
}
