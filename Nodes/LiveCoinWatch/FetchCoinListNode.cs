using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.LiveCoinWatch
{
    [NodeDefinition("FetchCoinListNode", "Fetch Coin List", NodeTypeEnum.Function, "LiveCoinWatch")]
    [NodeGraphDescription("Fetch coin list on LiveCoinWatch")]
    [NodeIDEParameters(Hidden = true)]
    public class FetchCoinListNode : Node
    {
        public FetchCoinListNode(string id, BlockGraph graph)
            : base(id, graph, typeof(FetchCoinListNode).Name)
        {
            this.InParameters.Add("liveCoinWatch", new NodeParameter(this, "liveCoinWatch", typeof(LiveCoinWatchConnectorNode), true));
            this.InParameters.Add("currency", new NodeParameter(this, "currency", typeof(string), true));
            this.InParameters.Add("sort", new NodeParameter(this, "sort", typeof(string), true));
            this.InParameters.Add("order", new NodeParameter(this, "order", typeof(string), true));
            this.InParameters.Add("offset", new NodeParameter(this, "offset", typeof(int), true));
            this.InParameters.Add("limit", new NodeParameter(this, "limit", typeof(int), true));

            this.OutParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), false));
            this.OutParameters.Add("rate", new NodeParameter(this, "rate", typeof(double), false));
            this.OutParameters.Add("volume", new NodeParameter(this, "volume", typeof(int), false));
            this.OutParameters.Add("cap", new NodeParameter(this, "cap", typeof(int), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            LiveCoinWatchConnectorNode liveCoinWatchConnectorNode = this.InParameters["liveCoinWatch"].GetValue() as LiveCoinWatchConnectorNode;

            var coinRequest = liveCoinWatchConnectorNode.API.FetchListCoin(
                this.InParameters["currency"].GetValue().ToString(),
                this.InParameters["sort"].GetValue().ToString(),
                this.InParameters["order"].GetValue().ToString(),
                this.InParameters["limit"].GetValue(),
                this.InParameters["offset"].GetValue()
                
            );
            coinRequest.Wait();

            this.OutParameters["symbol"].SetValue(coinRequest.Result.Symbol);
            this.OutParameters["rate"].SetValue(coinRequest.Result.Rate);
            this.OutParameters["volume"].SetValue(coinRequest.Result.Volume);
            this.OutParameters["cap"].SetValue(coinRequest.Result.Cap);

            return true;
        }
    }
}
