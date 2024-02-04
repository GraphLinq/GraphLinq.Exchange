using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.CoinbaseAccounts
{
    [NodeDefinition("GetAllCoinbaseAccountsNode", "Get All Coinbase Accounts", NodeTypeEnum.Function, "Coinbase Pro Coinbase Accounts")]
    [NodeGraphDescription("This node retrieves all Coinbase accounts linked to the Coinbase Pro account.")]
    public class GetAllCoinbaseAccountsNode : Node
    {
        public GetAllCoinbaseAccountsNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetAllCoinbaseAccountsNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));

            this.OutParameters.Add("coinbaseAccounts", new NodeParameter(this, "coinbaseAccounts", typeof(List<object>), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            var coinbaseAccounts = await coinbaseProConnector.Client.CoinbaseAccounts.GetAllAccountsAsync();

            this.OutParameters["coinbaseAccounts"].SetValue(coinbaseAccounts);
            return true;
        }
    }
}
