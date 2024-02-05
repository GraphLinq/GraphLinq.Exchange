using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.Deposits
{
    [NodeDefinition("GetDepositNode", "Get Coinbase Pro Deposit Information", NodeTypeEnum.Function, "Coinbase Pro Deposits")]
    [NodeGraphDescription("This node retrieves information for a single deposit on Coinbase Pro using the deposit ID.")]
    public class GetDepositNode : Node
    {
        public GetDepositNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetDepositNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
            this.InParameters.Add("depositId", new NodeParameter(this, "depositId", typeof(string), true));

            this.OutParameters.Add("depositInfo", new NodeParameter(this, "depositInfo", typeof(object), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            var depositId = this.InParameters["depositId"].GetValue().ToString();

            var depositInfo = await coinbaseProConnector.Client.Deposits.GetDeposit(depositId);

            this.OutParameters["depositInfo"].SetValue(depositInfo);
            return true;
        }
    }
}
