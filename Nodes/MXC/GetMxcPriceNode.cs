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

            this.OutParameters.Add("bid_price", new NodeParameter(this, "bid_price", typeof(decimal), false));
            this.OutParameters.Add("bid_quantity", new NodeParameter(this, "bid_quantity", typeof(decimal), false));

            this.OutParameters.Add("ask_price", new NodeParameter(this, "ask_price", typeof(decimal), false));
            this.OutParameters.Add("ask_quantity", new NodeParameter(this, "ask_quantity", typeof(decimal), false));

        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            MxcConnectorNode connector = this.InParameters["mxc"].GetValue() as MxcConnectorNode;
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("symbol", "ETH_USDT");
            param.Add("depth", "1");

            var result = connector.Client.Get<MarketPriceEntity>("/open/api/v2/market/depth", param);

            this.OutParameters["ask_price"].SetValue(result.data.asks[0].price);
            this.OutParameters["ask_quantity"].SetValue(result.data.asks[0].quantity);

            this.OutParameters["bid_price"].SetValue(result.data.bids[0].price);
            this.OutParameters["bid_quantity"].SetValue(result.data.bids[0].quantity);

            return true;
        }
    }
}
