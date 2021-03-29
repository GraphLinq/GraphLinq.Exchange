using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.Kraken.Events
{
    [NodeDefinition("OnKrakenTickerUpdateNode", "On Kraken Pair Ticker Update", NodeTypeEnum.Event, "Kraken")]
    [NodeGraphDescription("Get a event when the book ticker is updated on Kraken for a symbol. Symbol example : BTC/USD")]
    [NodeCycleLimit(1000)]
    public class OnKrakenTickerUpdateNode : Node
    {
        public OnKrakenTickerUpdateNode(string id, BlockGraph graph)
            : base(id, graph, typeof(OnKrakenTickerUpdateNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("kraken", new NodeParameter(this, "kraken", typeof(KrakenConnectorNode), true));
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
            KrakenConnectorNode connector = this.InParameters["kraken"].GetValue() as KrakenConnectorNode;
            connector.SocketClient.SubscribeToTickerUpdates(this.InParameters["symbol"].GetValue().ToString(), (data) =>
            {
                var instanciatedParameters = this.InstanciateParametersForCycle();
                instanciatedParameters["bestAskPrice"].SetValue((double)data.Data.BestAsks.Price);
                instanciatedParameters["bestAskQuantity"].SetValue((double)data.Data.BestAsks.Quantity);
                instanciatedParameters["bestBidPrice"].SetValue((double)data.Data.BestBids.Price);
                instanciatedParameters["bestBidQuantity"].SetValue((double)data.Data.BestBids.Quantity);
                this.Graph.AddCycle(this, instanciatedParameters);
            });
        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
