using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.Orders
{
    [NodeDefinition("GetAllOrdersNode", "Get All Coinbase Pro Orders", NodeTypeEnum.Function, "Coinbase Pro Orders")]
    [NodeGraphDescription("This node retrieves all open orders associated with the Coinbase Pro account.")]
    public class GetAllOrdersNode : Node
    {
        public GetAllOrdersNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetAllOrdersNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));

            this.OutParameters.Add("orders", new NodeParameter(this, "orders", typeof(List<object>), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            var orders = await coinbaseProConnector.Client.Orders.GetAllOrdersAsync();

            this.OutParameters["orders"].SetValue(orders);
            return true;
        }
    }
}
