using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro
{
    [NodeDefinition("OnCoinbaseProPairTickerUpdateNode", "On CoinbasePro Pair Ticker Update", NodeTypeEnum.Event, "CoinbasePro")]
    [NodeGraphDescription("Get a event when the book ticker is updated on CoinbasePro for a symbol")]
    [NodeCycleLimit(1000)]
    public class OnCoinbaseProPairTickerUpdateNode : Node
    {
        public OnCoinbaseProPairTickerUpdateNode(string id, BlockGraph graph)
              : base(id, graph, typeof(OnCoinbaseProPairTickerUpdateNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));

            this.OutParameters.Add("message", new NodeParameter(this," message", typeof(string), false));
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            CoinbaseProConnectorNode coinbaseConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            coinbaseConnector.SocketClient.Spot.SubscribeToBookTickerUpdates(this.InParameters["connection"].GetValue(), (data) =>
            {
                var instanciatedParameters = this.InstanciatedParametersForCycle();
                instanciatedParameters["message"].SetValue((string)data.Message);
                this.Graph.AddCycle(this, instanciatedParameters);
            });
        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
