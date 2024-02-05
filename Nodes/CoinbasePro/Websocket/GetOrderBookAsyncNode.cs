using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.MarketData
{
    [NodeDefinition("GetOrderBookAsyncNode", "Get Coinbase Pro Order Book", NodeTypeEnum.Function, "Coinbase Pro Market Data")]
    [NodeGraphDescription("This node provides a snapshot of the order book for a given product on Coinbase Pro.")]
    public class GetOrderBookAsyncNode : Node
    {
        public GetOrderBookAsyncNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetOrderBookAsyncNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
            this.InParameters.Add("productId", new NodeParameter(this, "productId", typeof(string), true));
            this.InParameters.Add("level", new NodeParameter(this, "level", typeof(int), false));

            this.OutParameters.Add("orderBook", new NodeParameter(this, "orderBook", typeof(object), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            var productId = this.InParameters["productId"].GetValue().ToString();
            var level = (int)this.InParameters["level"].GetValue();

            var orderBook = await coinbaseProConnector.Client.MarketData.GetOrderBookAsync(productId, level);

            this.OutParameters["orderBook"].SetValue(orderBook);
            return true;
        }
    }
}
