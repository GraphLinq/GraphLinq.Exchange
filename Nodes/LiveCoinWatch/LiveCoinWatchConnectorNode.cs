using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.LiveCoinWatch
{
    [NodeDefinition("LiveCoinWatchConnectorNode", "LiveCoinWatch Connector", NodeTypeEnum.Connector, "LiveCoinWatch")]
    [NodeGraphDescription("LiveCoinWatch Connector")]
    [NodeSpecialActionAttribute("Go to LiveCoinWatch API Portal", "open_url", "https://www.livecoinwatch.com/tools/api")]
    [NodeIDEParameters(Hidden = false)]
    public class LiveCoinWatchConnectorNode : Node
    {
        public LiveCoinWatchConnectorNode(string id, BlockGraph graph)
           : base(id, graph, typeof(LiveCoinWatchConnectorNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("apiKey", new NodeParameter(this, "apiKey", typeof(string), true));

            this.OutParameters.Add("liveCoinWatch", new NodeParameter(this, "liveCoinWatch", typeof(LiveCoinWatchConnectorNode), true));
        }

        public API.LiveCoinWatchAPI API { get; set; }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            this.API = new API.LiveCoinWatchAPI(this.InParameters["apiKey"].GetValue().ToString());

            this.Next();
        }

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "liveCoinWatch")
            {
                return this;
            }

            return base.ComputeParameterValue(parameter, value);
        }
    }
}
