using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.MXC.Entities
{
    [Serializable]
    public class BalanceEntity
    {
        public int code
        {
            get; set;
        }

        public dynamic data
        {
            get;set;
        }
    }

   
}
