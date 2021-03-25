using Kucoin.Net;
using Kucoin.Net.Objects;
using Newtonsoft.Json;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.KuCoin
{
    [NodeDefinition("KuCoinConnectorNode", "KuCoin Connector", NodeTypeEnum.Connector, "KuCoin")]
    [NodeGraphDescription("Kucoin connector")]
    public class KuCoinConnectorNode : Node
    {
        public KuCoinConnectorNode(string id, BlockGraph graph)
            : base(id, graph, typeof(KuCoinConnectorNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("apiKey", new NodeParameter(this, "apiKey", typeof(string), true));
            this.InParameters.Add("apiSecret", new NodeParameter(this, "apiSecret", typeof(string), true));
            this.InParameters.Add("apiPassphrase", new NodeParameter(this, "apiPassphrase", typeof(string), true));

            this.OutParameters.Add("kucoin", new NodeParameter(this, "kucoin", typeof(KuCoinConnectorNode), true));
        }

        [JsonIgnore]
        public KucoinClient Client { get; set; }

        [JsonIgnore]
        public KucoinSocketClient SocketClient { get; set; }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            this.Client = new KucoinClient(new KucoinClientOptions()
            {
                ApiCredentials = new KucoinApiCredentials(
                    this.InParameters["apiKey"].GetValue().ToString(),
                    this.InParameters["apiSecret"].GetValue().ToString(),
                    this.InParameters["apiPassphrase"].GetValue().ToString())
            });
            this.SocketClient = new KucoinSocketClient();
            this.Next();
        }

        public override void OnStop()
        {
            this.Client.Dispose();
            this.SocketClient.Dispose();
        }

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "kucoin")
            {
                return this;
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
