using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using NodeBlock.Plugin.Exchange.Nodes.MXC.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.MXC
{
    [NodeDefinition("GetMxcPriceNode.", "Get MXC Price", NodeTypeEnum.Function, "MXC")]
    [NodeGraphDescription("Get the current price on MXC for a symbol. Symbol example : ETH_USDT")]
    public class GetMxcPriceNode : Node
    {
        public GetMxcPriceNode(string id, BlockGraph graph)
          : base(id, graph, typeof(GetMxcPriceNode).Name)
        {
            this.InParameters.Add("mxc", new NodeParameter(this, "mxc", typeof(MxcConnectorNode), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));

            this.OutParameters.Add("hight", new NodeParameter(this, "hight", typeof(decimal), false));
            this.OutParameters.Add("low", new NodeParameter(this, "low", typeof(decimal), false));
            this.OutParameters.Add("bid", new NodeParameter(this, "bid", typeof(decimal), false));
            this.OutParameters.Add("ask", new NodeParameter(this, "ask", typeof(decimal), false));

        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            MxcConnectorNode connector = this.InParameters["mxc"].GetValue() as MxcConnectorNode;
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("symbol", "ETH_USDT");
            param.Add("depth", "1");

            var result = connector.Client.Get<MarketPrice>("/open/api/v2/market/depth", param);
            // todo : assign output
            return true;
        }
    }
}
