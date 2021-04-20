using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.MXC.Entities
{
    [Serializable]
    public class MarketPriceEntity
    {
        public int code
        {
            get;set;
        }

        public DataInformation data
        {
            get;set;
        }

    }

    [Serializable]
    public class DataInformation
    {
        public List<Details> asks
        {
            get;set;
        }
        public List<Details> bids
        {
            get; set;
        }

    }

    [Serializable]
    public class Details
    {
        public decimal price
        {
            get;set;
        }

        public decimal quantity
        {
            get;set;
        }
    }
}
