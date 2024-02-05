using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.MarketData
{
    [NodeDefinition("GetStatsAsyncNode", "Get Coinbase Pro 24-Hour Stats", NodeTypeEnum.Function, "Coinbase Pro Market Data")]
    [NodeGraphDescription("This node provides 24-hour statistics for a specified product on Coinbase Pro.")]
    public class GetStatsAsyncNode : Node
    {
        public GetStatsAsyncNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetStatsAsyncNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
            this.InParameters.Add("productId", new NodeParameter(this, "productId", typeof(string), true));

            this.OutParameters.Add("stats", new NodeParameter(this, "stats", typeof(object), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            var productId = this.InParameters["productId"].GetValue().ToString();

            var stats = await coinbaseProConnector.Client.MarketData.GetStatsAsync(productId);

            this.OutParameters["stats"].SetValue(stats);
            return true;
        }
    }
}
