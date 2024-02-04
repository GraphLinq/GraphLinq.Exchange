using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.Withdrawals
{
    [NodeDefinition("WithdrawFundsToCryptoAddressNode", "Withdraw Funds to Crypto Address from Coinbase Pro", NodeTypeEnum.Function, "Coinbase Pro Withdrawals")]
    [NodeGraphDescription("This node withdraws funds to a specified crypto address from the Coinbase Pro account.")]
    public class WithdrawFundsToCryptoAddressNode : Node
    {
        public WithdrawFundsToCryptoAddressNode(string id, BlockGraph graph)
            : base(id, graph, typeof(WithdrawFundsToCryptoAddressNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
            this.InParameters.Add("amount", new NodeParameter(this, "amount", typeof(decimal), true));
            this.InParameters.Add("currency", new NodeParameter(this, "currency", typeof(string), true));
            this.InParameters.Add("cryptoAddress", new NodeParameter(this, "cryptoAddress", typeof(string), true));
            this.InParameters.Add("destinationTag", new NodeParameter(this, "destinationTag", typeof(string), false)); // Optional, for currencies that require it
            this.InParameters.Add("noDestinationTag", new NodeParameter(this, "noDestinationTag", typeof(bool), false)); // Optional, to explicitly state no tag is needed

            this.OutParameters.Add("withdrawal", new NodeParameter(this, "withdrawal", typeof(object), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            var amount = (string)this.InParameters["amount"].GetValue().ToString();
            var currency = (decimal)this.InParameters["currency"].GetValue();
            var cryptoAddress = this.InParameters["cryptoAddress"].GetValue().ToString();
            var destinationTag = this.InParameters["destinationTag"].GetValue().ToString();
            var noDestinationTag = bool.Parse(this.InParameters["noDestinationTag"].GetValue().ToString());

            var withdrawal = await coinbaseProConnector.Client.Withdrawals.WithdrawFundsToCryptoAddressAsync(
                amount, currency, cryptoAddress, destinationTag: destinationTag, noDestinationTag: noDestinationTag);

            this.OutParameters["withdrawal"].SetValue(withdrawal);
            return true;
        }
    }
}
