using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.Deposits
{
    [NodeDefinition("ListDepositsNode", "List Coinbase Pro Deposits", NodeTypeEnum.Function, "Coinbase Pro Deposits")]
    [NodeGraphDescription("This node lists all deposit transactions on Coinbase Pro.")]
    public class ListDepositsNode : Node
    {
        public ListDepositsNode(string id, BlockGraph graph)
            : base(id, graph, typeof(ListDepositsNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));

            this.OutParameters.Add("deposits", new NodeParameter(this, "deposits", typeof(List<object>), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;

            var deposits = await coinbaseProConnector.Client.Deposits.ListDeposits();

            this.OutParameters["deposits"].SetValue(deposits);
            return true;
        }
    }
}
