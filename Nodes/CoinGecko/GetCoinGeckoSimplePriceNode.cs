using CoinGecko.Clients;
using CoinGecko.Interfaces;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinGecko
{
    [NodeDefinition("GetCoinGeckoSimplePriceNode", "Get CoinGecko Coin", NodeTypeEnum.Function, "CoinGecko")]
    [NodeGraphDescription("Get CoinGecko Coin Data")]
    public class GetCoinGeckoSimplePriceNode : Node
    {
        private readonly ICoinGeckoClient _client = CoinGeckoClient.Instance;

        public GetCoinGeckoSimplePriceNode(string id, BlockGraph graph)
               : base(id, graph, typeof(GetCoinGeckoSimplePriceNode).Name)
        {
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));

            this.OutParameters.Add("price", new NodeParameter(this, "price", typeof(double), false));
            this.OutParameters.Add("marketCapChange24H", new NodeParameter(this, "marketCapChange24H", typeof(double), false));
        }
        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var request = this._client.CoinsClient.GetAllCoinDataWithId(this.InParameters["symbol"].GetValue().ToString());
            request.Wait();

            var result = request.Result;
            this.OutParameters["price"].SetValue(result.MarketData.CurrentPrice["usd"]);
            this.OutParameters["marketCapChange24H"].SetValue(result.MarketData.MarketCapChangePercentage24H);
            return true;
        }
    }
}
