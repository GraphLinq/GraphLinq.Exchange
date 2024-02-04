/*
using Coinbase.Pro.WebSockets;
using Coinbase.Pro.Models;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.WebSocket
    [NodeDefinition("SubscribeToTickerUpdatesNode", "Subscribe to Coinbase Pro Ticker Updates", NodeTypeEnum.Event, "Coinbase Pro WebSocket")]
    [NodeGraphDescription("This node subscribes to ticker updates for specified products on Coinbase Pro using WebSockets.")]
    [NodeCycleLimit(1000)]
    public class SubscribeToTickerUpdatesNode : Node
    {
        public SubscribeToTickerUpdatesNode(string id, BlockGraph graph)
              : base(id, graph, typeof(SubscribeToTickerUpdatesNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
            this.InParameters.Add("productIds", new NodeParameter(this, "productIds", typeof(string), true)); // Comma-separated list

            this.OutParameters.Add("productId", new NodeParameter(this, "productId", typeof(string), false));
            this.OutParameters.Add("price", new NodeParameter(this, "price", typeof(decimal), false));
            this.OutParameters.Add("volume", new NodeParameter(this, "volume", typeof(decimal), false));
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            var productIds = this.InParameters["productIds"].GetValue().ToString().Split(',');

            coinbaseProConnector.SocketClient. += (sender, e) =>
            {
                var message = JsonSerializer.Deserialize<Dictionary<string, object>>(e.Json);
                if (message != null && message.ContainsKey("type") && message["type"].ToString() == "ticker")
                {
                    string productId = message["product_id"].ToString();
                    if (Array.IndexOf(productIds, productId) > -1)
                    {
                        decimal price = decimal.Parse(message["price"].ToString());
                        decimal volume = decimal.Parse(message["volume_24h"].ToString());

                        var instanciatedParameters = this.InstanciatedParametersForCycle();
                        instanciatedParameters["productId"].SetValue(productId);
                        instanciatedParameters["price"].SetValue(price);
                        instanciatedParameters["volume"].SetValue(volume);
                        this.Graph.AddCycle(this, instanciatedParameters);
                    }
                }
            };

            Task.Run(async () =>
            {
                await coinbaseProConnector.SocketClient.ConnectAsync();
                await coinbaseProConnector.SocketClient.SubscribeAsync(productIds);
            }).Wait();
        }

        public override void BeginCycle()
        {
            this.Next();
        }

    }
}
*/
