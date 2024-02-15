using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using Coinbase.Models;
using Newtonsoft.Json;

namespace NodeBlock.Plugin.Exchange.Nodes.Coinbase.Data
{
    [NodeDefinition("GetCoinbaseBuyPriceNode", "Get Coinbase Buy Price", NodeTypeEnum.Function, "Coinbase")]
    [NodeGraphDescription("Get the current buy price on Coinbase")]
    public class GetCoinbaseBuyPriceNode : Node
    {
        public GetCoinbaseBuyPriceNode(string id, BlockGraph graph)
              : base(id, graph, typeof(GetCoinbaseBuyPriceNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseConnectorNode), true));
            this.InParameters.Add("currencyPair", new NodeParameter(this, "currencyPair", typeof(string), true));

            this.OutParameters.Add("amount", new NodeParameter(this, "amount", typeof(decimal), false));
            this.OutParameters.Add("currency", new NodeParameter(this, "currency", typeof(string), false));
            this.OutParameters.Add("base", new NodeParameter(this, "base", typeof(string), false));
        }

        [JsonIgnore]
        public JsonResponse Money { get; private set; }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            CoinbaseConnectorNode coinbaseConnector = this.InParameters["connection"].GetValue() as CoinbaseConnectorNode;

            var request = coinbaseConnector.Client.Data.GetBuyPriceAsync(this.InParameters["currencyPair"].GetValue().ToString());
            request.Wait();

            var response = new Money
            {
                Amount = request.Result.Data.Amount,
                Currency = request.Result.Data.Currency,
                Base = request.Result.Data.Base
            };

            this.OutParameters["amount"].SetValue(response.Amount);
            this.OutParameters["currency"].SetValue(response.Currency);
            this.OutParameters["base"].SetValue(response.Base);

            return true;
        }
    }
}
