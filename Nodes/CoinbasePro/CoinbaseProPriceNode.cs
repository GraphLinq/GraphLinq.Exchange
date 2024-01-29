using Coinbase;
using Flurl;
using Flurl.Http;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.Coinbase
{
    [NodeDefinition("CoinbasePriceNode", "Get Coinbase Price", NodeTypeEnum.Function, "Coinbase")]
    [NodeGraphDescription("Get the current price on Coinbase for a symbol")]
    public class CoinbasePriceNode : Node
    {
        public CoinbasePriceNode(string id, BlockGraph graph)
              : base(id, graph, typeof(CoinbasePriceNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseConnectorNode), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));

            this.OutParameters.Add("price", new NodeParameter(this, "price", typeof(double), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            CoinbaseConnectorNode coinbaseConnector = this.InParameters["connection"].GetValue() as CoinbaseConnectorNode;

            var request = coinbaseConnector.Client.Data.GetSpotPriceAsync(this.InParameters["symbol"].GetValue().ToString());
            request.Wait();

            this.OutParameters["price"].SetValue(request.Result.Data.Amount);
            return true;
        }
    }
}
