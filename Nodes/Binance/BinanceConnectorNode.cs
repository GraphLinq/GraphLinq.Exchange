using Binance.Net;
using Binance.Net.Objects.Spot;
using Newtonsoft.Json;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.Binance
{
    [NodeDefinition("BinanceConnectorNode", "Binance Connector", NodeTypeEnum.Connector, "Binance")]
    [NodeGraphDescription("Binance connector, for retrieve your ApiKey and ApiSecret go to your Binance account and create them")]
    public class BinanceConnectorNode : Node
    {
        public BinanceConnectorNode(string id, BlockGraph graph)
           : base(id, graph, typeof(BinanceConnectorNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("apiKey", new NodeParameter(this, "apiKey", typeof(string), true));
            this.InParameters.Add("apiSecret", new NodeParameter(this, "apiSecret", typeof(string), true));

            this.OutParameters.Add("connection", new NodeParameter(this, "connection", typeof(BinanceConnectorNode), true));
        }

        [JsonIgnore]
        public BinanceClient Client { get; set; }

        [JsonIgnore]
        public BinanceSocketClient SocketClient { get; set; }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            this.Client = new BinanceClient(new BinanceClientOptions
            {
                ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials(this.InParameters["apiKey"].GetValue().ToString(), this.InParameters["apiSecret"].GetValue().ToString())
            });
            this.SocketClient = new BinanceSocketClient();
            this.Next();
        }

        public override void OnStop()
        {
            this.Client.Dispose();
            this.SocketClient.Dispose();
        }

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "connection")
            {
                return this;
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
