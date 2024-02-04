using Coinbase;
using Flurl;
using Flurl.Http;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro
{
    [NodeDefinition("CoinbaseProPriceNode", "Get CoinbasePro Price", NodeTypeEnum.Function, "CoinbasePro")]
    [NodeGraphDescription("Get the current price on Coinbase pro for a symbol")]
    public class CoinbaseProPriceNode : Node
    {
        public CoinbaseProPriceNode(string id, BlockGraph graph)
              : base(id, graph, typeof(CoinbaseProPriceNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));

            this.OutParameters.Add("price", new NodeParameter(this, "price", typeof(double), false));
            this.OutParameters.Add("open", new NodeParameter(this, "open", typeof(double), false));
            this.OutParameters.Add("high", new NodeParameter(this, "high", typeof(double), false));
            this.OutParameters.Add("low", new NodeParameter(this, "low", typeof(double), false));
            this.OutParameters.Add("volume", new NodeParameter(this, "volume", typeof(double), false));

        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;

            var request = coinbaseProConnector.Client.MarketData.GetStatsAsync(this.InParameters["symbol"].GetValue().ToString());
            request.Wait();

            this.OutParameters["price"].SetValue(request.Result.Last);
            this.OutParameters["open"].SetValue(request.Result.Open);
            this.OutParameters["high"].SetValue(request.Result.High);
            this.OutParameters["low"].SetValue(request.Result.Low);
            this.OutParameters["volume"].SetValue(request.Result.Volume);


            return true;
        }
    }
}
