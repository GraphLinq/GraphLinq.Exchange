using Bitfinex.Net;
using Bitfinex.Net.Objects;
using Bittrex.Net;
using Bittrex.Net.Objects;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NodeBlock.Plugin.Exchange.Nodes.Bitfinex
{
    [NodeDefinition("BitfinexConnectorNode","Bitfinex Connector",NodeTypeEnum.Connector, "Bitfinex")]
    [NodeGraphDescription("Bitfinex Connector, for retrieve your ApiKey and ApiSecret go to your Bitfinex account and create them")]
    public class BitfinexConnectorNode : Node
    {
        public BitfinexConnectorNode(string id,BlockGraph graph)
            : base(id, graph, typeof(BitfinexConnectorNode).Name)
        {
            this.CanBeSerialized = false;


            this.InParameters.Add("apiKey", new NodeParameter(this, "apiKey", typeof(string), true));
            this.InParameters.Add("apiSecret", new NodeParameter(this, "apiSecret", typeof(string), true));

            this.OutParameters.Add("bitfinex", new NodeParameter(this, "bitfinex", typeof(BitfinexConnectorNode), true));
        }


        [JsonIgnore]
        public BitfinexClient Client { get; set; }


        [JsonIgnore]
        public BitfinexSocketClient SocketClient { get; set; }
        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            this.Client = new BitfinexClient(new BitfinexClientOptions()
            {
                ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials(this.InParameters["apiKey"].GetValue().ToString(), this.InParameters["apiSecret"].GetValue().ToString())
            });

            this.SocketClient = new BitfinexSocketClient();

            this.Next();
        }

        public override void OnStop()
        {
            this.Client.Dispose();
            this.SocketClient.Dispose();
        }


        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "bitfinex")
            {
                return this;
            }
            return base.ComputeParameterValue(parameter, value);
        }

    }
}
