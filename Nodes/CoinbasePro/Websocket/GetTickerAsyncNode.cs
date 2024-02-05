using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.MarketData
{
    [NodeDefinition("GetTickerAsyncNode", "Get Coinbase Pro Ticker Information", NodeTypeEnum.Function, "Coinbase Pro Market Data")]
    [NodeGraphDescription("This node retrieves the latest ticker information for a specified product on Coinbase Pro.")]
    public class GetTickerAsyncNode : Node
    {
        public GetTickerAsyncNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetTickerAsyncNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
            this.InParameters.Add("productId", new NodeParameter(this, "productId", typeof(string), true));

            this.OutParameters.Add("tickerInfo", new NodeParameter(this, "tickerInfo", typeof(object), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            var productId = this.InParameters["productId"].GetValue().ToString();

            var tickerInfo = await coinbaseProConnector.Client.MarketData.GetTickerAsync(productId);

            this.OutParameters["tickerInfo"].SetValue(tickerInfo);
            return true;
        }
    }
}
