using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.Deposits
{
    [NodeDefinition("DepositFundsFromCoinbaseAccountNode", "Deposit Funds from Coinbase Account to Coinbase Pro", NodeTypeEnum.Function, "Coinbase Pro Deposits")]
    [NodeGraphDescription("This node deposits funds from a Coinbase account into the Coinbase Pro account.")]
    public class DepositFundsFromCoinbaseAccountNode : Node
    {
        public DepositFundsFromCoinbaseAccountNode(string id, BlockGraph graph)
            : base(id, graph, typeof(DepositFundsFromCoinbaseAccountNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
            this.InParameters.Add("coinbaseAccountId", new NodeParameter(this, "coinbaseAccountId", typeof(string), true));
            this.InParameters.Add("amount", new NodeParameter(this, "amount", typeof(decimal), true));
            this.InParameters.Add("currency", new NodeParameter(this, "currency", typeof(string), true));

            this.OutParameters.Add("deposit", new NodeParameter(this, "deposit", typeof(object), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            var deposit = await coinbaseProConnector.Client.Deposits.DepositFundsFromCoinbaseAccountAsync(
                this.InParameters["coinbaseAccountId"].GetValue().ToString(),
                (decimal)this.InParameters["amount"].GetValue(),
                this.InParameters["currency"].GetValue().ToString()
            );

            this.OutParameters["deposit"].SetValue(deposit);
            return true;
        }
    }
}
