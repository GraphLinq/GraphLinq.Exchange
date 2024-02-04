using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.Accounts
{
    [NodeDefinition("GetAccountNode", "Get Coinbase Pro Account By ID", NodeTypeEnum.Function, "Coinbase Pro Accounts")]
    [NodeGraphDescription("This node retrieves a specific Coinbase Pro account by its ID.")]
    public class GetAccountNode : Node
    {
        public GetAccountNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetAccountNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
            this.InParameters.Add("accountId", new NodeParameter(this, "accountId", typeof(string), true));

            this.OutParameters.Add("account", new NodeParameter(this, "account", typeof(object), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            string accountId = this.InParameters["accountId"].GetValue().ToString();

            var account = await coinbaseProConnector.Client.Accounts.GetAccountAsync(accountId);

            this.OutParameters["account"].SetValue(account);
            return true;
        }
    }
}
