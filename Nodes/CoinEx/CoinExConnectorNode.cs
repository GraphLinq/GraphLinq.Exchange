using CoinEx.Net;
using CoinEx.Net.Objects;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinEx
{
    [NodeDefinition("CoinExConnectorNode", "CoinEx Connector", NodeTypeEnum.Connector, "CoinEx")]
    [NodeGraphDescription("CoinEx Connector, for retrieve your ApiKey and ApiSecret go to your CoinEx account and create them")]
    public class CoinExConnectorNode : Node
    {
        public CoinExConnectorNode(string id, BlockGraph graph)
            : base(id, graph, typeof(CoinExConnectorNode).Name)
        {
            this.CanBeSerialized = false;


            this.InParameters.Add("apiKey", new NodeParameter(this, "apiKey", typeof(string), true));
            this.InParameters.Add("apiSecret", new NodeParameter(this, "apiSecret", typeof(string), true));

            this.OutParameters.Add("coinEx", new NodeParameter(this, "coinEx", typeof(CoinExConnectorNode), true));
        }


        [JsonIgnore]
        public CoinExClient Client { get; set; }


        [JsonIgnore]
        public CoinExSocketClient SocketClient { get; set; }
        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            this.Client = new CoinExClient(new CoinExClientOptions()
            {
                ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials(this.InParameters["apiKey"].GetValue().ToString(), this.InParameters["apiSecret"].GetValue().ToString())
            });

            this.SocketClient = new CoinExSocketClient();
            this.Next();
        }

        public override void OnStop()
        {
            this.Client.Dispose();
            this.SocketClient.Dispose();
        }


        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "coinEx")
            {
                return this;
            }
            return base.ComputeParameterValue(parameter, value);
        }

    }
}
