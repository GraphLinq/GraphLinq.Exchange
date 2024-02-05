using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.MarketData
{
    [NodeDefinition("GetTradesAsyncNode", "Get Coinbase Pro Latest Trades", NodeTypeEnum.Function, "Coinbase Pro Market Data")]
    [NodeGraphDescription("This node lists the latest trades for a specified product on Coinbase Pro.")]
    public class GetTradesAsyncNode : Node
    {
        public GetTradesAsyncNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetTradesAsyncNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
            this.InParameters.Add("productId", new NodeParameter(this, "productId", typeof(string), true));

            this.OutParameters.Add("trades", new NodeParameter(this, "trades", typeof(List<object>), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            var productId = this.InParameters["productId"].GetValue().ToString();

            var trades = await coinbaseProConnector.Client.MarketData.GetTradesAsync(productId);

            this.OutParameters["trades"].SetValue(trades);
            return true;
        }
    }
}
