using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.LiveCoinWatch
{
    [NodeDefinition("FetchOverviewNode", "Fetch Market Overview", NodeTypeEnum.Function, "LiveCoinWatch")]
    [NodeGraphDescription("Fetch market overview on LiveCoinWatch")]
    [NodeIDEParameters(Hidden = true)]
    public class FetchOverviewNode : Node
    {
        public FetchOverviewNode(string id, BlockGraph graph)
            : base(id, graph, typeof(FetchOverviewNode).Name)
        {
            this.InParameters.Add("liveCoinWatch", new NodeParameter(this, "liveCoinWatch", typeof(LiveCoinWatchConnectorNode), true));
            this.InParameters.Add("currency", new NodeParameter(this, "currency", typeof(string), true));

            this.OutParameters.Add("liquidity", new NodeParameter(this, "liquidity", typeof(int), false));
            this.OutParameters.Add("btcDominance", new NodeParameter(this, "btcDominance", typeof(double), false));
            this.OutParameters.Add("volume", new NodeParameter(this, "volume", typeof(int), false));
            this.OutParameters.Add("cap", new NodeParameter(this, "cap", typeof(int), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            LiveCoinWatchConnectorNode liveCoinWatchConnectorNode = this.InParameters["liveCoinWatch"].GetValue() as LiveCoinWatchConnectorNode;

            var coinRequest = liveCoinWatchConnectorNode.API.FetchOverview(this.InParameters["currency"].GetValue().ToString());
            coinRequest.Wait();

            this.OutParameters["liquidity"].SetValue(coinRequest.Result.Liquidity);
            this.OutParameters["btcDominance"].SetValue(coinRequest.Result.btcDominance);
            this.OutParameters["volume"].SetValue(coinRequest.Result.Volume);
            this.OutParameters["cap"].SetValue(coinRequest.Result.Cap);

            return true;
        }
    }
}
