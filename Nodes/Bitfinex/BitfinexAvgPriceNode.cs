using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Bitfinex.Net.Objects;

namespace NodeBlock.Plugin.Exchange.Nodes.Bitfinex
{
    [NodeDefinition("BitfinexAvgPriceNode", "Get Bitfinex Average Price", NodeTypeEnum.Function, "Bitfinex")]
    [NodeGraphDescription("Get the current average price on bitfinex for a symbol. Symbol example : tETHUSD or tBTCUSD")]
    public class BitfinexAvgPriceNode : Node
    {
        public BitfinexAvgPriceNode(string id, BlockGraph graph)
          : base(id, graph, typeof(BitfinexAvgPriceNode).Name)
        {
            this.InParameters.Add("bitfinex", new NodeParameter(this, "bitfinex", typeof(BitfinexConnectorNode), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));


            this.OutParameters.Add("averagePrice", new NodeParameter(this, "averagePrice", typeof(double), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            BitfinexConnectorNode bitfinexConnector = this.InParameters["bitfinex"].GetValue() as BitfinexConnectorNode;
            var symbol = this.InParameters["symbol"].GetValue().ToString();
            var result =  bitfinexConnector.Client.GetAveragePrice(symbol, 1m, 1m).Data;
            this.OutParameters["averagePrice"].SetValue(result.AverageRate);
            return true;
        }
    }
}
