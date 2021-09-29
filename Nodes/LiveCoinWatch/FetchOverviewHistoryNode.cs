using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.LiveCoinWatch
{
    [NodeDefinition("FetchOverviewHistoryNode", "Fetch Overview History", NodeTypeEnum.Function, "LiveCoinWatch")]
    [NodeGraphDescription("Fetch market overview history on LiveCoinWatch")]
    [NodeIDEParameters(Hidden = true)]
    public class FetchOverviewHistoryNode : Node
    {
        public FetchOverviewHistoryNode(string id, BlockGraph graph)
            : base(id, graph, typeof(FetchOverviewHistoryNode).Name)
        {
            this.InParameters.Add("liveCoinWatch", new NodeParameter(this, "liveCoinWatch", typeof(LiveCoinWatchConnectorNode), true));
            this.InParameters.Add("currency", new NodeParameter(this, "currency", typeof(string), true));
            this.InParameters.Add("start", new NodeParameter(this, "start", typeof(int), true));
            this.InParameters.Add("end", new NodeParameter(this, "end", typeof(int), true));

            this.OutParameters.Add("history", new NodeParameter(this, "history", typeof(array), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            LiveCoinWatchConnectorNode liveCoinWatchConnectorNode = this.InParameters["liveCoinWatch"].GetValue() as LiveCoinWatchConnectorNode;

            var coinRequest = liveCoinWatchConnectorNode.API.FetchHistoryOverview(
                this.InParameters["currency"].GetValue().ToString(),
                this.InParameters["start"].GetValue(),
                this.InParameters["end"].GetValue()
            );
            coinRequest.Wait();

            this.OutParameters["history"].SetValue(coinRequest.Result.History);

            return true;
        }
    }
}
