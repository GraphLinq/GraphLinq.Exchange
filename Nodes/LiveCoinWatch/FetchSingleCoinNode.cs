using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.LiveCoinWatch
{
    [NodeDefinition("FetchSingleCoinNode", "Fetch Single Coin", NodeTypeEnum.Function, "LiveCoinWatch")]
    [NodeGraphDescription("Fetch coin informations on LiveCoinWatch")]
    [NodeIDEParameters(Hidden = true)]
    public class FetchSingleCoinNode : Node
    {
        public FetchSingleCoinNode(string id, BlockGraph graph)
            : base(id, graph, typeof(FetchSingleCoinNode).Name)
        {
            this.InParameters.Add("liveCoinWatch", new NodeParameter(this, "liveCoinWatch", typeof(LiveCoinWatchConnectorNode), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));

            this.OutParameters.Add("allTimeHighUSD", new NodeParameter(this, "allTimeHighUSD", typeof(double), false));
            this.OutParameters.Add("circulatingSupply", new NodeParameter(this, "circulatingSupply", typeof(int), false));
            this.OutParameters.Add("totalSupply", new NodeParameter(this, "totalSupply", typeof(int), false));
            this.OutParameters.Add("maxSupply", new NodeParameter(this, "maxSupply", typeof(int), false));
            this.OutParameters.Add("rate", new NodeParameter(this, "rate", typeof(double), false));
            this.OutParameters.Add("volume", new NodeParameter(this, "volume", typeof(int), false));
            this.OutParameters.Add("cap", new NodeParameter(this, "cap", typeof(int), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            LiveCoinWatchConnectorNode liveCoinWatchConnectorNode = this.InParameters["liveCoinWatch"].GetValue() as LiveCoinWatchConnectorNode;

            var coinRequest = liveCoinWatchConnectorNode.API.FetchCoinSingle("USD", this.InParameters["symbol"].GetValue().ToString());
            coinRequest.Wait();

            this.OutParameters["allTimeHighUSD"].SetValue(coinRequest.Result.AllTimeHighUSD);
            this.OutParameters["circulatingSupply"].SetValue(coinRequest.Result.CirculatingSupply);
            this.OutParameters["totalSupply"].SetValue(coinRequest.Result.TotalSupply);
            this.OutParameters["maxSupply"].SetValue(coinRequest.Result.MaxSupply);
            this.OutParameters["rate"].SetValue(coinRequest.Result.Rate);
            this.OutParameters["volume"].SetValue(coinRequest.Result.Volume);
            this.OutParameters["cap"].SetValue(coinRequest.Result.Cap);

            return true;
        }
    }
}
