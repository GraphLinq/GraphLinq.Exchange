using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.LiveCoinWatch
{
    [NodeDefinition("FetchSingleCoinHistoryNode", "Fetch Single Coin History", NodeTypeEnum.Function, "LiveCoinWatch")]
    [NodeGraphDescription("Fetch coin history on LiveCoinWatch")]
    [NodeIDEParameters(Hidden = true)]
    public class FetchSingleCoinHistoryNode : Node
    {
        public FetchSingleCoinHistoryNode(string id, BlockGraph graph)
            : base(id, graph, typeof(FetchSingleCoinHistoryNode).Name)
        {
            this.InParameters.Add("liveCoinWatch", new NodeParameter(this, "liveCoinWatch", typeof(LiveCoinWatchConnectorNode), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));
            this.InParameters.Add("currency", new NodeParameter(this, "currency", typeof(string), true));
            this.InParameters.Add("start", new NodeParameter(this, "start", typeof(int), true));
            this.InParameters.Add("end", new NodeParameter(this, "end", typeof(int), true));

            
            this.OutParameters.Add("allTimeHighUSD", new NodeParameter(this, "allTimeHighUSD", typeof(double), false));
            this.OutParameters.Add("circulatingSupply", new NodeParameter(this, "circulatingSupply", typeof(int), false));
            this.OutParameters.Add("totalSupply", new NodeParameter(this, "totalSupply", typeof(int), false));
            this.OutParameters.Add("maxSupply", new NodeParameter(this, "maxSupply", typeof(int), false));
            this.OutParameters.Add("rate", new NodeParameter(this, "rate", typeof(double), false));
            this.OutParameters.Add("volume", new NodeParameter(this, "volume", typeof(int), false));
            this.OutParameters.Add("cap", new NodeParameter(this, "cap", typeof(int), false));
            this.OutParameters.Add("name", new NodeParameter(this, "name", typeof(string), false));
            this.OutParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), false));
            this.OutParameters.Add("png32", new NodeParameter(this, "png32", typeof(string), false));
            this.OutParameters.Add("png64", new NodeParameter(this, "png64", typeof(string), false));
            this.OutParameters.Add("exchanges", new NodeParameter(this, "exchanges", typeof(int), false));
            this.OutParameters.Add("markets", new NodeParameter(this, "markets", typeof(int), false));
            this.OutParameters.Add("pairs", new NodeParameter(this, "pairs", typeof(int), false));
            this.OutParameters.Add("history", new NodeParameter(this, "history", typeof(string), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            LiveCoinWatchConnectorNode liveCoinWatchConnectorNode = this.InParameters["liveCoinWatch"].GetValue() as LiveCoinWatchConnectorNode;

            var coinRequest = liveCoinWatchConnectorNode.API.FetchCoinSingleHistory(
                this.InParameters["currency"].GetValue().ToString(),
                this.InParameters["symbol"].GetValue().ToString(),
                this.InParameters["start"].GetValue(),
                this.InParameters["end"].GetValue()
            );
            coinRequest.Wait();

            this.OutParameters["allTimeHighUSD"].SetValue(coinRequest.Result.AllTimeHighUSD);
            this.OutParameters["circulatingSupply"].SetValue(coinRequest.Result.CirculatingSupply);
            this.OutParameters["totalSupply"].SetValue(coinRequest.Result.TotalSupply);
            this.OutParameters["maxSupply"].SetValue(coinRequest.Result.MaxSupply);
            this.OutParameters["rate"].SetValue(coinRequest.Result.Rate);
            this.OutParameters["volume"].SetValue(coinRequest.Result.Volume);
            this.OutParameters["cap"].SetValue(coinRequest.Result.Cap);
            this.OutParameters["name"].SetValue(coinRequest.Result.Name);
            this.OutParameters["symbol"].SetValue(coinRequest.Result.Symbol);
            this.OutParameters["png32"].SetValue(coinRequest.Result.Png32);
            this.OutParameters["png64"].SetValue(coinRequest.Result.Png64);
            this.OutParameters["exchanges"].SetValue(coinRequest.Result.Exchanges);
            this.OutParameters["markets"].SetValue(coinRequest.Result.Markets);
            this.OutParameters["pairs"].SetValue(coinRequest.Result.Pairs);
            this.OutParameters["history"].SetValue(coinRequest.Result.History);

            return true;
        }
    }
}
