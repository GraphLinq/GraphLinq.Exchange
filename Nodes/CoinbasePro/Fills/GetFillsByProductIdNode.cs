using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.Fills
{
    [NodeDefinition("GetFillsByProductIdNode", "Get Coinbase Pro Fills by Product ID", NodeTypeEnum.Function, "Coinbase Pro Fills")]
    [NodeGraphDescription("This node retrieves all fills for a specific product associated with the Coinbase Pro account.")]
    public class GetFillsByProductIdNode : Node
    {
        public GetFillsByProductIdNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetFillsByProductIdNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
            this.InParameters.Add("productId", new NodeParameter(this, "productId", typeof(string), true));

            this.OutParameters.Add("fills", new NodeParameter(this, "fills", typeof(List<object>), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            string productId = this.InParameters["productId"].GetValue().ToString();

            var fills = await coinbaseProConnector.Client.Fills.GetFillsByProductIdAsync(productId);

            this.OutParameters["fills"].SetValue(fills);
            return true;
        }
    }
}
