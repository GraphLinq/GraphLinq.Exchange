using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.Withdrawals
{
    [NodeDefinition("WithdrawFundsToCoinbaseAccountNode", "Withdraw Funds to Coinbase Account from Coinbase Pro", NodeTypeEnum.Function, "Coinbase Pro Withdrawals")]
    [NodeGraphDescription("This node withdraws funds to a Coinbase account from the Coinbase Pro account.")]
    public class WithdrawFundsToCoinbaseAccountNode : Node
    {
        public WithdrawFundsToCoinbaseAccountNode(string id, BlockGraph graph)
            : base(id, graph, typeof(WithdrawFundsToCoinbaseAccountNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
            this.InParameters.Add("coinbaseAccountId", new NodeParameter(this, "coinbaseAccountId", typeof(string), true));
            this.InParameters.Add("amount", new NodeParameter(this, "amount", typeof(decimal), true));
            this.InParameters.Add("currency", new NodeParameter(this, "currency", typeof(string), true));

            this.OutParameters.Add("withdrawal", new NodeParameter(this, "withdrawal", typeof(object), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            var withdrawal = await coinbaseProConnector.Client.Withdrawals.WithdrawFundsToCoinbaseAsync(
                this.InParameters["coinbaseAccountId"].GetValue().ToString(),
                (decimal)this.InParameters["amount"].GetValue(),
                this.InParameters["currency"].GetValue().ToString()
            );

            this.OutParameters["withdrawal"].SetValue(withdrawal);
            return true;
        }
    }
}
