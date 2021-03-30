using Huobi.Net;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NodeBlock.Plugin.Exchange.Nodes.Huobi
{
    [NodeDefinition("HuobiConnectorNode", "Huobi Connector", NodeTypeEnum.Connector, "Huobi")]
    [NodeGraphDescription("Huobi connector")]
    public class HuobiConnectorNode : Node
    {
        public HuobiConnectorNode(string id, BlockGraph graph)
           : base(id, graph, typeof(HuobiConnectorNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("apiKey", new NodeParameter(this, "apiKey", typeof(string), true));
            this.InParameters.Add("apiSecret", new NodeParameter(this, "apiSecret", typeof(string), true));

            this.OutParameters.Add("huobi", new NodeParameter(this, "huobi", typeof(HuobiConnectorNode), true));
        }

        [JsonIgnore]
        public HuobiClient Client { get; set; }

        [JsonIgnore]
        public HuobiSocketClient SocketClient { get; set; }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            this.Client = new HuobiClient(new HuobiClientOptions()
            {
                ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials(this.InParameters["apiKey"].GetValue().ToString(), this.InParameters["apiSecret"].GetValue().ToString())
            });
            this.SocketClient = new HuobiSocketClient();
            this.Next();
        }

        public override void OnStop()
        {
            this.Client.Dispose();
            this.SocketClient.Dispose();
        }

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "huobi")
            {
                return this;
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
