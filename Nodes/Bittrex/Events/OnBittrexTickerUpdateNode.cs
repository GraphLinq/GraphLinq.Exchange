using Bitfinex.Net.Objects;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NodeBlock.Plugin.Exchange.Nodes.Bittrex.Events
{
    [NodeDefinition("OnBittrexTickerUpdateNode", "On Bittrex Pair Ticker Update", NodeTypeEnum.Event, "Bittrex")]
    [NodeGraphDescription("Get a event when the book ticker is updated on Bittrex for a symbol")]
    [NodeCycleLimit(1000)]
    public class OnBittrexTickerUpdateNode : Node
    {
        public OnBittrexTickerUpdateNode(string id, BlockGraph graph)
              : base(id, graph, typeof(OnBittrexTickerUpdateNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("bittrex", new NodeParameter(this, "bittrex", typeof(BittrexConnectorNode), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));

 
            this.OutParameters.Add("bestAskPrice", new NodeParameter(this, "bestAskPrice", typeof(decimal), false));
            this.OutParameters.Add("bestBidPrice", new NodeParameter(this, "bestBidPrice", typeof(decimal), false));
            this.OutParameters.Add("lastTradeRate", new NodeParameter(this, "lastTradeRate", typeof(decimal), false));

        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            BittrexConnectorNode bittrexConnector = this.InParameters["bittrex"].GetValue() as BittrexConnectorNode;
            var result = bittrexConnector.SocketClient.SubscribeToSymbolTickerUpdatesAsync(this.InParameters["symbol"].GetValue().ToString(), (data) =>
            {
                var instanciatedParameters = this.InstanciatedParametersForCycle();
                instanciatedParameters["bestAskPrice"].SetValue(data.AskRate);
                instanciatedParameters["bestBidPrice"].SetValue(data.BidRate);
                instanciatedParameters["lastTradeRate"].SetValue(data.LastTradeRate);

                this.Graph.AddCycle(this, instanciatedParameters);
            });

            result.Wait();

        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
