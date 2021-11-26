using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.LiveCoinWatch
{
    [NodeDefinition("FetchFiatAllNode", "Fetch All Fiat", NodeTypeEnum.Function, "LiveCoinWatch")]
    [NodeGraphDescription("Fetch fiat informations on LiveCoinWatch")]
    [NodeIDEParameters(Hidden = true)]
    public class FetchFiatAllNode : Node
    {
        public FetchFiatAllNode(string id, BlockGraph graph)
            : base(id, graph, typeof(FetchFiatAllNode).Name)
        {
            this.InParameters.Add("liveCoinWatch", new NodeParameter(this, "liveCoinWatch", typeof(LiveCoinWatchConnectorNode), true));

            this.OutParameters.Add("code", new NodeParameter(this, "code", typeof(string), false));
            this.OutParameters.Add("countries", new NodeParameter(this, "countries", typeof(string), false));
            this.OutParameters.Add("name", new NodeParameter(this, "name", typeof(string), false));
            this.OutParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), false));
            this.OutParameters.Add("flag", new NodeParameter(this, "flag", typeof(string), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            LiveCoinWatchConnectorNode liveCoinWatchConnectorNode = this.InParameters["liveCoinWatch"].GetValue() as LiveCoinWatchConnectorNode;

            var coinRequest = liveCoinWatchConnectorNode.API.FetchAllFiat();
            coinRequest.Wait();

            this.OutParameters["code"].SetValue(coinRequest.Result.Code);
            this.OutParameters["countries"].SetValue(coinRequest.Result.Countries);
            this.OutParameters["symbol"].SetValue(coinRequest.Result.Symbol);
            this.OutParameters["flag"].SetValue(coinRequest.Result.Flag);

            return true;
        }
    }
}
