using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.PaymentMethods
{
    [NodeDefinition("GetAllPaymentMethodsNode", "Get All Coinbase Pro Payment Methods", NodeTypeEnum.Function, "Coinbase Pro Payment Methods")]
    [NodeGraphDescription("This node retrieves all payment methods linked to the Coinbase Pro account.")]
    public class GetAllPaymentMethodsNode : Node
    {
        public GetAllPaymentMethodsNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetAllPaymentMethodsNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));

            this.OutParameters.Add("paymentMethods", new NodeParameter(this, "paymentMethods", typeof(List<object>), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            var paymentMethods = await coinbaseProConnector.Client.PaymentMethods.GetAllPaymentMethodsAsync();

            this.OutParameters["paymentMethods"].SetValue(paymentMethods);
            return true;
        }
    }
}
