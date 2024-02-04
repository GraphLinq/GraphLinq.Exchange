using Coinbase.Pro.WebSockets;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using NodeBlock.Plugin.Exchange.Nodes.Binance;
using System;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.MarketData
{
    [NodeDefinition("MarketDataWebSocketNode", "Subscribe to Coinbase Pro Market Data", NodeTypeEnum.Function, "Coinbase Pro Market Data")]
    [NodeGraphDescription("This node subscribes to market data updates such as ticker information for specified products using Coinbase Pro WebSockets.")]
    public class MarketDataWebSocketNode : Node
    {
        public MarketDataWebSocketNode(string id, BlockGraph graph)
            : base(id, graph, typeof(MarketDataWebSocketNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
            this.InParameters.Add("productIds", new NodeParameter(this, "productIds", typeof(string), true));

            this.OutParameters.Add("message", new NodeParameter(this, "message", typeof(string), false));
        }

        private CoinbaseProWebSocket SocketClient;

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
         {

             this.Next();
         } 

        public override void SetupEvent()
        {
            this.Next();
        }

        public override void BeginCycle()
        {
            this.Next();
        }

    }
}
