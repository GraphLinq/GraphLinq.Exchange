using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.Accounts
{
    [NodeDefinition("GetAllAccountsNode", "Get All Coinbase Pro Accounts", NodeTypeEnum.Function, "Coinbase Pro Accounts")]
    [NodeGraphDescription("This node retrieves all accounts associated with the Coinbase Pro account.")]
    public class GetAllAccountsNode : Node
    {
        public GetAllAccountsNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetAllAccountsNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));

            this.OutParameters.Add("accounts", new NodeParameter(this, "accounts", typeof(object), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            var accounts = await coinbaseProConnector.Client.Accounts.GetAllAccountsAsync();

            this.OutParameters["accounts"].SetValue(accounts);
            return true;
        }
    }
}
