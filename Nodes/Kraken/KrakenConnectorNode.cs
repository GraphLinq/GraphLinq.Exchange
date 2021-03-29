using Kraken.Net;
using Newtonsoft.Json;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.Kraken
{
    [NodeDefinition("KrakenConnectorNode", "Kraken Connector", NodeTypeEnum.Connector, "Kraken")]
    [NodeGraphDescription("Kraken connector")]
    public class KrakenConnectorNode : Node
    {
        public KrakenConnectorNode(string id, BlockGraph graph)
           : base(id, graph, typeof(KrakenConnectorNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("apiKey", new NodeParameter(this, "apiKey", typeof(string), true));
            this.InParameters.Add("apiSecret", new NodeParameter(this, "apiSecret", typeof(string), true));

            this.OutParameters.Add("kraken", new NodeParameter(this, "kraken", typeof(KrakenConnectorNode), true));
        }

        [JsonIgnore]
        public KrakenClient Client { get; set; }

        [JsonIgnore]
        public KrakenSocketClient SocketClient { get; set; }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            this.Client = new KrakenClient(new KrakenClientOptions()
            {
                ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials(this.InParameters["apiKey"].GetValue().ToString(), this.InParameters["apiSecret"].GetValue().ToString())
            });
            this.SocketClient = new KrakenSocketClient();
            this.Next();
        }

        public override void OnStop()
        {
            this.Client.Dispose();
            this.SocketClient.Dispose();
        }

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "kraken")
            {
                return this;
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
