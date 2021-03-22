using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.Binance
{
    [NodeDefinition("BinanceGetVolumeNode", "Get Binance Volume", NodeTypeEnum.Function, "Binance")]
    [NodeGraphDescription("Get the 24h volume on binance for a symbol")]
    public class BinanceGetVolumeNode : Node
    {
        public BinanceGetVolumeNode(string id, BlockGraph graph)
                 : base(id, graph, typeof(BinanceGetVolumeNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(BinanceConnectorNode), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));

            this.OutParameters.Add("baseVolume", new NodeParameter(this, "baseVolume", typeof(double), false));
            this.OutParameters.Add("quoteVolume", new NodeParameter(this, "quoteVolume", typeof(double), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            BinanceConnectorNode binanceConnector = this.InParameters["connection"].GetValue() as BinanceConnectorNode;
            var result = binanceConnector.Client.Spot.Market.Get24HPrice(this.InParameters["symbol"].GetValue().ToString());
            this.OutParameters["baseVolume"].SetValue((double)result.Data.BaseVolume);
            this.OutParameters["quoteVolume"].SetValue((double)result.Data.QuoteVolume);

            return true;
        }
    }
}
