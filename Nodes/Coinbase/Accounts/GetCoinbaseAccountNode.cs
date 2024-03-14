using Coinbase.Models;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;

namespace NodeBlock.Plugin.Exchange.Nodes.Coinbase.Accounts
{
    [NodeDefinition("GetCoinbaseAccountNode", "Get Coinbase Account", NodeTypeEnum.Function, "Coinbase")]
    [NodeGraphDescription("Get an account on Coinbase")]
    public class GetCoinbaseAccountNode : Node
    {
        public GetCoinbaseAccountNode(string id, BlockGraph graph)
              : base(id, graph, typeof(GetCoinbaseAccountNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseConnectorNode), true));
            this.InParameters.Add("accountId", new NodeParameter(this, "accountId", typeof(string), true));

            this.OutParameters.Add("name", new NodeParameter(this, "name", typeof(string), false));
            this.OutParameters.Add("amount", new NodeParameter(this, "amount", typeof(decimal), false));
            this.OutParameters.Add("currency", new NodeParameter(this, "currency", typeof(string), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            CoinbaseConnectorNode coinbaseConnector = this.InParameters["connection"].GetValue() as CoinbaseConnectorNode;

            var request = coinbaseConnector.Client.Accounts.GetAccountAsync(this.InParameters["accountId"].GetValue().ToString());
            request.Wait();

            var response = new Account
            {
                Name = request.Result.Data.Name,
                Primary = request.Result.Data.Primary,
                Type = request.Result.Data.Type,
                Currency = request.Result.Data.Currency,
                Balance = request.Result.Data.Balance
            };

            var accountCurrency = new AccountCurrency
            {
                Code = response.Currency.Code,
                Name = response.Currency.Name,
                Color= response.Currency.Name,
                SortIndex = response.Currency.SortIndex,
                Exponent = response.Currency.Exponent,
                Type = response.Currency.Type,
                AddressRegex = response.Currency.AddressRegex,
                AssetId = response.Currency.AssetId
            };

            var balance = new Money
            {
                Amount = response.Balance.Amount,
                Currency = response.Balance.Currency,
                Base = response.Balance.Base
            };

            this.OutParameters["name"].SetValue(accountCurrency.Name);
            this.OutParameters["amount"].SetValue(balance.Amount);
            this.OutParameters["currency"].SetValue(balance.Currency);

            return true;
        }
    }
}
