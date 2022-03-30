using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.KuCoin.Trade
{
    [NodeDefinition("GetKuCoinTickerPriceNode", "Get KuCoin Ticker Price", NodeTypeEnum.Function, "KuCoin")]
    [NodeGraphDescription("Get the price from KuCoin Exchange")]
    public class GetKuCoinTickerPriceNode : Node
    {
        public GetKuCoinTickerPriceNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetKuCoinTickerPriceNode).Name)
        {
            this.InParameters.Add("kucoin", new NodeParameter(this, "kucoin", typeof(object), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));

            this.OutParameters.Add("bestAsk", new NodeParameter(this, "bestAsk", typeof(decimal), false));
            this.OutParameters.Add("bestAskQuantity", new NodeParameter(this, "bestAskQuantity", typeof(decimal), false));
            this.OutParameters.Add("bestBid", new NodeParameter(this, "bestBid", typeof(decimal), false));
            this.OutParameters.Add("lastTradePrice", new NodeParameter(this, "lastTradePrice", typeof(decimal), false));
            this.OutParameters.Add("lastTradeQuantity", new NodeParameter(this, "lastTradeQuantity", typeof(decimal), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            KuCoinConnectorNode connector = this.InParameters["kucoin"].GetValue() as KuCoinConnectorNode;
            var symbol = this.InParameters["symbol"].GetValue().ToString();

            var result = connector.Client.GetTickerAsync(symbol);

            this.OutParameters["bestAsk"].SetValue(result.Result.Data.BestAsk);
            this.OutParameters["bestAskQuantity"].SetValue(result.Result.Data.BestAskQuantity);
            this.OutParameters["bestBid"].SetValue(result.Result.Data.BestBid);
            this.OutParameters["lastTradePrice"].SetValue(result.Result.Data.LastTradePrice);
            this.OutParameters["lastTradeQuantity"].SetValue(result.Result.Data.LastTradeQuantity);

            return true;
        }
    }
}
