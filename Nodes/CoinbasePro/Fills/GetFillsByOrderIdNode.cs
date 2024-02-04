using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.Fills
{
    [NodeDefinition("GetFillsByOrderIdNode", "Get Coinbase Pro Fills by Order ID", NodeTypeEnum.Function, "Coinbase Pro Fills")]
    [NodeGraphDescription("This node retrieves all fills for a specific order associated with the Coinbase Pro account.")]
    public class GetFillsByOrderIdNode : Node
    {
        public GetFillsByOrderIdNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetFillsByOrderIdNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
            this.InParameters.Add("orderId", new NodeParameter(this, "orderId", typeof(string), true));

            this.OutParameters.Add("fills", new NodeParameter(this, "fills", typeof(List<object>), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            string orderId = this.InParameters["orderId"].GetValue().ToString();

            var fills = await coinbaseProConnector.Client.Fills.GetFillsByOrderIdAsync(orderId);

            this.OutParameters["fills"].SetValue(fills);
            return true;
        }
    }
}
