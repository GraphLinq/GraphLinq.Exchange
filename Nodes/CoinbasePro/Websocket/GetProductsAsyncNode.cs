using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.MarketData
{
    [NodeDefinition("GetProductsAsyncNode", "Get Coinbase Pro Products", NodeTypeEnum.Function, "Coinbase Pro Market Data")]
    [NodeGraphDescription("This node lists available currency pairs for trading on Coinbase Pro.")]
    public class GetProductsAsyncNode : Node
    {
        public GetProductsAsyncNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetProductsAsyncNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));

            this.OutParameters.Add("products", new NodeParameter(this, "products", typeof(List<object>), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;

            var products = await coinbaseProConnector.Client.MarketData.GetProductsAsync();

            this.OutParameters["products"].SetValue(products);
            return true;
        }
    }
}
