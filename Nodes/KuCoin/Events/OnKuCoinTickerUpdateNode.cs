using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.KuCoin.Events
{
    [NodeDefinition("OnKuCoinTickerUpdateNode", "On KuCoin Pair Ticker Update", NodeTypeEnum.Event, "KuCoin")]
    [NodeGraphDescription("Get a event when the book ticker is updated on KuCoin for a symbol")]
    [NodeCycleLimit(1000)]
    public class OnKuCoinTickerUpdateNode : Node
    {
        public OnKuCoinTickerUpdateNode(string id, BlockGraph graph)
            : base(id, graph, typeof(OnKuCoinTickerUpdateNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("kucoin", new NodeParameter(this, "kucoin", typeof(KuCoinConnectorNode), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));

            this.OutParameters.Add("bestAskPrice", new NodeParameter(this, "bestAskPrice", typeof(double), false));
            this.OutParameters.Add("bestAskQuantity", new NodeParameter(this, "bestAskQuantity", typeof(double), false));
            this.OutParameters.Add("bestBidPrice", new NodeParameter(this, "bestBidPrice", typeof(double), false));
            this.OutParameters.Add("bestBidQuantity", new NodeParameter(this, "bestBidQuantity", typeof(double), false));
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            KuCoinConnectorNode connector = this.InParameters["kucoin"].GetValue() as KuCoinConnectorNode;
            connector.SocketClient.SubscribeToTickerUpdates(this.InParameters["symbol"].GetValue().ToString(), (data) =>
            {
                var instanciatedParameters = this.InstanciateParametersForCycle();
                instanciatedParameters["bestAskPrice"].SetValue((double)data.BestAsk);
                instanciatedParameters["bestAskQuantity"].SetValue((double)data.BestAskQuantity);
                instanciatedParameters["bestBidPrice"].SetValue((double)data.BestBid);
                instanciatedParameters["bestBidQuantity"].SetValue((double)data.BestBidQuantity);
                this.Graph.AddCycle(this, instanciatedParameters);
            });
        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
