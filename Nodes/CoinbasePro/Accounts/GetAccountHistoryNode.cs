using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.Accounts
{
    [NodeDefinition("GetAccountHistoryNode", "Get Coinbase Pro Account History", NodeTypeEnum.Function, "Coinbase Pro Accounts")]
    [NodeGraphDescription("This node retrieves the history of account activities for a specified account ID on Coinbase Pro.")]
    public class GetAccountHistoryNode : Node
    {
        public GetAccountHistoryNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetAccountHistoryNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
            this.InParameters.Add("accountId", new NodeParameter(this, "accountId", typeof(string), true));

            this.OutParameters.Add("accountHistory", new NodeParameter(this, "accountHistory", typeof(object), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            string accountId = this.InParameters["accountId"].GetValue().ToString();

            var accountHistory = await coinbaseProConnector.Client.Accounts.GetAccountHistoryAsync(accountId);

            this.OutParameters["accountHistory"].SetValue(accountHistory);
            return true;
        }
    }
}
