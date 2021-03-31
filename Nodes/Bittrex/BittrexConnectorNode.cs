
using Bittrex.Net;
using Bittrex.Net.Objects;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NodeBlock.Plugin.Exchange.Nodes.Bittrex
{
    [NodeDefinition("BittrexConnectorNode","Bittrex Connector",NodeTypeEnum.Connector, "Bittrex")]
    [NodeGraphDescription("Bittrex Connector, for retrieve your ApiKey and ApiSecret go to your Bittrex account and create them")]
    public class BittrexConnectorNode : Node
    {
        public BittrexConnectorNode(string id,BlockGraph graph)
            : base(id, graph, typeof(BittrexConnectorNode).Name)
        {
            this.CanBeSerialized = false;


            this.InParameters.Add("apiKey", new NodeParameter(this, "apiKey", typeof(string), true));
            this.InParameters.Add("apiSecret", new NodeParameter(this, "apiSecret", typeof(string), true));

            this.OutParameters.Add("bittrex", new NodeParameter(this, "bittrex", typeof(BittrexConnectorNode), true));
        }


        [JsonIgnore]
        public BittrexClient Client { get; set; }


        [JsonIgnore]
        public BittrexSocketClient SocketClient { get; set; }
        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            this.Client = new BittrexClient(new BittrexClientOptions()
            {
                ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials(this.InParameters["apiKey"].GetValue().ToString(), this.InParameters["apiSecret"].GetValue().ToString())
            });

            this.SocketClient = new BittrexSocketClient();

            this.Next();
        }

        public override void OnStop()
        {
            this.Client.Dispose();
            this.SocketClient.Dispose();
        }


        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "bittrex")
            {
                return this;
            }
            return base.ComputeParameterValue(parameter, value);
        }

    }
}
