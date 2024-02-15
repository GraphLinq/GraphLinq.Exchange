using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using Coinbase.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace NodeBlock.Plugin.Exchange.Nodes.Coinbase.Data
{
    [NodeDefinition("GetCoinbaseExchangeRateNode", "Get Coinbase Exchange Rate", NodeTypeEnum.Function, "Coinbase")]
    [NodeGraphDescription("Get the current exchange rate on Coinbase")]
    public class GetCoinbaseExchangeRateNode : Node
    {
        public GetCoinbaseExchangeRateNode(string id, BlockGraph graph)
              : base(id, graph, typeof(GetCoinbaseExchangeRateNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseConnectorNode), true));
            this.InParameters.Add("currencyPair", new NodeParameter(this, "currencyPair", typeof(string), true));

            this.OutParameters.Add("currency", new NodeParameter(this, "currency", typeof(string), false));
            this.OutParameters.Add("rates", new NodeParameter(this, "rates", typeof(Dictionary<string, decimal>), false));
        }

        [JsonIgnore]
        public JsonResponse Rates { get; private set; }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            CoinbaseConnectorNode coinbaseConnector = this.InParameters["connection"].GetValue() as CoinbaseConnectorNode;

            var request = coinbaseConnector.Client.Data.GetExchangeRatesAsync(this.InParameters["currencyPair"].GetValue().ToString());
            request.Wait();

            var response = new ExchangeRates
            {
                Rates = request.Result.Data.Rates,
                Currency = request.Result.Data.Currency
            };

            this.OutParameters["amount"].SetValue(response.Rates);
            this.OutParameters["currency"].SetValue(response.Currency);

            return true;
        }
    }
}
