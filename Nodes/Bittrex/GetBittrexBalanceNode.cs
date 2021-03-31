using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.Bittrex
{
    [NodeDefinition("GetBittrexBalanceNode.", "Get Bittrex Balance", NodeTypeEnum.Function, "Bittrex")]
    [NodeGraphDescription("Get the balance on your account. Symbol example : BTC")]
    public class GetBittrexBalanceNode : Node
    {
        public GetBittrexBalanceNode(string id, BlockGraph graph)
          : base(id, graph, typeof(GetBittrexBalanceNode).Name)
        {
            this.InParameters.Add("bittrex", new NodeParameter(this, "bittrex", typeof(BittrexConnectorNode), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));


            this.OutParameters.Add("available", new NodeParameter(this, "available", typeof(decimal), false));
            this.OutParameters.Add("total", new NodeParameter(this, "total", typeof(decimal), false));

        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            BittrexConnectorNode bitfinexConnector = this.InParameters["bittrex"].GetValue() as BittrexConnectorNode;
            var symbol = this.InParameters["symbol"].GetValue().ToString();
            var result = bitfinexConnector.Client.GetBalance(symbol);
            this.OutParameters["available"].SetValue(result.Data.Available);
            this.OutParameters["total"].SetValue(result.Data.Total);

            return true;
        }
    }
}
