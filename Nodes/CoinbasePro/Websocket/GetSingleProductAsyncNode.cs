using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.MarketData
{
    [NodeDefinition("GetSingleProductAsyncNode", "Get Coinbase Pro Single Product Data", NodeTypeEnum.Function, "Coinbase Pro Market Data")]
    [NodeGraphDescription("This node fetches market data for a specific currency pair on Coinbase Pro.")]
    public class GetSingleProductAsyncNode : Node
    {
        public GetSingleProductAsyncNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetSingleProductAsyncNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
            this.InParameters.Add("productId", new NodeParameter(this, "productId", typeof(string), true));

            this.OutParameters.Add("productData", new NodeParameter(this, "productData", typeof(object), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            var productId = this.InParameters["productId"].GetValue().ToString();

            var productData = await coinbaseProConnector.Client.MarketData.GetSingleProductAsync(productId);

            this.OutParameters["productData"].SetValue(productData);
            return true;
        }
    }
}
