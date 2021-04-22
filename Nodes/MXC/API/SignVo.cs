using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.MXC.API
{
    public class SignVo
    {
        public string ReqTime
        {
            get;set;
        }

        public string AcessKey
        {
            get;set;
        }

        public string SecretKey
        {
            get;set;
        }

        public string RequestParam
        {
            get;
            set;
        }
    }
}
