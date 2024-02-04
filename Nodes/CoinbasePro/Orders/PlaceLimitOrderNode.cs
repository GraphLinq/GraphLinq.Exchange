using Coinbase.Pro;
using Coinbase.Pro.Models;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.Orders
{
    [NodeDefinition("PlaceLimitOrderNode", "Place Coinbase Pro Limit Order", NodeTypeEnum.Function, "Coinbase Pro Orders")]
    [NodeGraphDescription("This node places a limit order on Coinbase Pro.")]
    public class PlaceLimitOrderNode : Node
    {
        public PlaceLimitOrderNode(string id, BlockGraph graph)
            : base(id, graph, typeof(PlaceLimitOrderNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
            this.InParameters.Add("productId", new NodeParameter(this, "productId", typeof(string), true));
            this.InParameters.Add("side", new NodeParameter(this, "side", typeof(string), true));
            this.InParameters.Add("price", new NodeParameter(this, "price", typeof(decimal), true));
            this.InParameters.Add("size", new NodeParameter(this, "size", typeof(decimal), true));

            this.OutParameters.Add("order", new NodeParameter(this, "order", typeof(Order), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            var productId = this.InParameters["productId"].GetValue().ToString();
            var side = this.InParameters["side"].GetValue().ToString().ToLower() == "buy" ? OrderSide.Buy : OrderSide.Sell;
            var price = (decimal)this.InParameters["price"].GetValue();
            var size = (decimal)this.InParameters["size"].GetValue();

            var order = await coinbaseProConnector.Client.Orders.PlaceLimitOrderAsync(side, productId, price, size);

            this.OutParameters["order"].SetValue(order);
            return true;
        }
    }
}
