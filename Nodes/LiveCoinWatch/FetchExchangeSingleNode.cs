using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.LiveCoinWatch
{
    [NodeDefinition("FetchExchangeSingleNode", "Fetch Single Exchange", NodeTypeEnum.Function, "LiveCoinWatch")]
    [NodeGraphDescription("Fetch single exchange informations on LiveCoinWatch")]
    [NodeIDEParameters(Hidden = true)]
    public class FetchExchangeSingleNode : Node
    {
        public FetchExchangeSingleNode(string id, BlockGraph graph)
            : base(id, graph, typeof(FetchExchangeSingleNode).Name)
        {
            this.InParameters.Add("liveCoinWatch", new NodeParameter(this, "liveCoinWatch", typeof(LiveCoinWatchConnectorNode), true));
            this.InParameters.Add("currency", new NodeParameter(this, "currency", typeof(string), true));
            this.InParameters.Add("exchange", new NodeParameter(this, "exchange", typeof(string), true));

            this.OutParameters.Add("volume", new NodeParameter(this, "volume", typeof(int), false));
            this.OutParameters.Add("png64", new NodeParameter(this, "png64", typeof(string), false));
            this.OutParameters.Add("png128", new NodeParameter(this, "png128", typeof(string), false));
            this.OutParameters.Add("markets", new NodeParameter(this, "markets", typeof(int), false));
            this.OutParameters.Add("bidTotal", new NodeParameter(this, "bidTotal", typeof(double), false));
            this.OutParameters.Add("askTotal", new NodeParameter(this, "askTotal", typeof(double), false));
            this.OutParameters.Add("depth", new NodeParameter(this, "depth", typeof(double), false));
            this.OutParameters.Add("visitors", new NodeParameter(this, "vistors", typeof(int), false));
            this.OutParameters.Add("volumePerVistor", new NodeParameter(this, "volumePerVisitor", typeof(double), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            LiveCoinWatchConnectorNode liveCoinWatchConnectorNode = this.InParameters["liveCoinWatch"].GetValue() as LiveCoinWatchConnectorNode;

            var coinRequest = liveCoinWatchConnectorNode.API.FetchSingleExchange(
                this.InParameters["currency"].GetValue().ToString(),
                this.InParameters["exchange"].GetValue().ToString(),
            );
            coinRequest.Wait();

            this.OutParameters["volume"].SetValue(coinRequest.Result.Volume);
            this.OutParameters["png64"].SetValue(coinRequest.Result.Png64);
            this.OutParameters["png128"].SetValue(coinRequest.Result.Png128);
            this.OutParameters["markets"].SetValue(coinRequest.Result.Markets);
            this.OutParameters["bidTotal"].SetValue(coinRequest.Result.BidTotal);
            this.OutParameters["askTotal"].SetValue(coinRequest.Result.AskTotal);
            this.OutParameters["depth"].SetValue(coinRequest.Result.Depth);
            this.OutParameters["vistors"].SetValue(coinRequest.Result.Visitors);
            this.OutParameters["volumePerVisitor"].SetValue(coinRequest.Result.volumePerVisitor);

            return true;
        }
    }
}
