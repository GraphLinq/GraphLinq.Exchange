/*
using Coinbase.Pro;
using Coinbase.Pro.Models;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.Accounts
{
    [NodeDefinition("GetAccountHoldNode", "Get Coinbase Pro Account Holds", NodeTypeEnum.Function, "Coinbase Pro Accounts")]
    [NodeGraphDescription("This node retrieves holds on an account related to active orders or pending withdraw requests for a specified account ID on Coinbase Pro.")]
    public class GetAccountHoldNode : Node
    {
        public GetAccountHoldNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetAccountHoldNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
            this.InParameters.Add("accountId", new NodeParameter(this, "accountId", typeof(string), true));

            this.OutParameters.Add("accountHolds", new NodeParameter(this, "accountHolds", typeof(object), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            string accountId = this.InParameters["accountId"].GetValue().ToString();

            var accountHolds = await coinbaseProConnector.Client.Accounts.GetAccountHoldsAsync(accountId);

            this.OutParameters["accountHolds"].SetValue(accountHolds);
            return true;
        }
    }
}
*/
