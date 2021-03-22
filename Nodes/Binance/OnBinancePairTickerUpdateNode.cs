using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.Binance
{
    [NodeDefinition("OnBinancePairTickerUpdateNode", "On Binance Pair Ticker Update", NodeTypeEnum.Event, "Binance")]
    [NodeGraphDescription("Get a event when the book ticker is updated on Binance for a symbol")]
    [NodeCycleLimit(1000)]
    public class OnBinancePairTickerUpdateNode : Node
    {
        public OnBinancePairTickerUpdateNode(string id, BlockGraph graph)
              : base(id, graph, typeof(OnBinancePairTickerUpdateNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(BinanceConnectorNode), true));
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
            BinanceConnectorNode binanceConnector = this.InParameters["connection"].GetValue() as BinanceConnectorNode;
            binanceConnector.SocketClient.Spot.SubscribeToBookTickerUpdates(this.InParameters["symbol"].GetValue().ToString(), (data) =>
            {
                var instanciatedParameters = this.InstanciateParametersForCycle();
                instanciatedParameters["bestAskPrice"].SetValue((double)data.BestAskPrice);
                instanciatedParameters["bestAskQuantity"].SetValue((double)data.BestAskQuantity);
                instanciatedParameters["bestBidPrice"].SetValue((double)data.BestBidPrice);
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
