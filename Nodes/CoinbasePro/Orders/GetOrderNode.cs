using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.Orders
{
    [NodeDefinition("GetOrderNode", "Get Coinbase Pro Order By ID", NodeTypeEnum.Function, "Coinbase Pro Orders")]
    [NodeGraphDescription("This node retrieves a specific Coinbase Pro order by its ID.")]
    public class GetOrderNode : Node
    {
        public GetOrderNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetOrderNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
            this.InParameters.Add("orderId", new NodeParameter(this, "orderId", typeof(string), true));

            this.OutParameters.Add("order", new NodeParameter(this, "order", typeof(object), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            string orderId = this.InParameters["orderId"].GetValue().ToString();

            var order = await coinbaseProConnector.Client.Orders.GetOrderAsync(orderId);

            this.OutParameters["order"].SetValue(order);
            return true;
        }
    }
}
