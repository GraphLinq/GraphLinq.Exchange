using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.Binance
{
    [NodeDefinition("BinanceAvgPriceNode", "Get Binance Average Price", NodeTypeEnum.Function, "Binance")]
    [NodeGraphDescription("Get the current average price on binance for a symbol")]
    public class BinanceAvgPriceNode : Node
    {
        public BinanceAvgPriceNode(string id, BlockGraph graph)
              : base(id, graph, typeof(BinanceAvgPriceNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(BinanceConnectorNode), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));

            this.OutParameters.Add("averagePrice", new NodeParameter(this, "averagePrice", typeof(double), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            BinanceConnectorNode binanceConnector = this.InParameters["connection"].GetValue() as BinanceConnectorNode;
            this.OutParameters["averagePrice"].SetValue((double)binanceConnector.Client.Spot.Market.GetCurrentAvgPrice(this.InParameters["symbol"].GetValue().ToString()).Data.Price);
            return true;
        }
    }
}
