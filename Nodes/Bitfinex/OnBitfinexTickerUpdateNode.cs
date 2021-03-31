using Bitfinex.Net.Objects;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NodeBlock.Plugin.Exchange.Nodes.Bitfinex
{
    [NodeDefinition("OnBitfinexTickerUpdateNode", "On Bitfinex Pair Ticker Update", NodeTypeEnum.Event, "Bitfinex")]
    [NodeGraphDescription("Get a event when the book ticker is updated on Bitfinex for a symbol")]
    [NodeCycleLimit(1000)]
    public class OnBitfinexTickerUpdateNode : Node
    {
        public OnBitfinexTickerUpdateNode(string id, BlockGraph graph)
              : base(id, graph, typeof(OnBitfinexTickerUpdateNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("bitfinex", new NodeParameter(this, "bitfinex", typeof(BitfinexConnectorNode), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));

 
            this.OutParameters.Add("bestPrice", new NodeParameter(this, "bestPrice", typeof(decimal), false));
            this.OutParameters.Add("bestQuantity", new NodeParameter(this, "bestQuantity", typeof(decimal), false));
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            BitfinexConnectorNode bitfinexConnector = this.InParameters["bitfinex"].GetValue() as BitfinexConnectorNode;
            bitfinexConnector.SocketClient.SubscribeToBookUpdates(this.InParameters["symbol"].GetValue().ToString(),Precision.PrecisionLevel0,Frequency.Realtime,100, (data) =>
            {
                var instanciatedParameters = this.InstanciateParametersForCycle();
                instanciatedParameters["bestPrice"].SetValue((decimal)data.ToList()[0].Price);
                instanciatedParameters["bestQuantity"].SetValue((decimal)data.ToList()[0].Quantity);
                this.Graph.AddCycle(this, instanciatedParameters);
            });
        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
