using Coinbase.Pro.Models;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using NodeBlock.Plugin.Exchange.Nodes.Binance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro
{
    [NodeDefinition("OnCoinbaseProUserChannelMsgNode", "On CoinbasePro User Channel Update Message", NodeTypeEnum.Event, "CoinbasePro")]
    [NodeGraphDescription("Get an event for the CoinbasePro WebSocket User Channel Message")]
    [NodeCycleLimit(1000)]
    public class OnCoinbaseProUserChannelMsgNode : Node
    {
        public OnCoinbaseProUserChannelMsgNode(string id, BlockGraph graph)
              : base(id, graph, typeof(OnCoinbaseProUserChannelMsgNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));

            this.OutParameters.Add("message", new NodeParameter(this," message", typeof(string), false));
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;


        public override async void SetupEvent()
        {
            var instanciatedParameters = this.InstanciatedParametersForCycle();
            //BinanceConnectorNode binanceConnector = this.InParameters["connection"].GetValue() as BinanceConnectorNode;
            CoinbaseProConnectorNode coinbaseConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            //coinbaseConnector.SocketClient.ConnectAsync();
            //coinbaseConnector.SocketClient
            var sub = new Subscription
            {
                ProductIds =
            {
            "BTC-USD",
            },
                Channels =
            {
            "heartbeat",
            }
            };

            //send the subscription upstream
            await coinbaseConnector.SocketClient.SubscribeAsync(sub);

            //now wait for data.
            await Task.Delay(TimeSpan.FromMinutes(1));

            this.Graph.AddCycle(this, instanciatedParameters);
        }

        public override void BeginCycle()
        {
            this.Next();
        }

    }
}
