using Coinbase.Pro;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro
{
    [NodeDefinition("CoinbaseProConnectorNode", "CoinbasePro Connector", NodeTypeEnum.Connector, "CoinbasePro")]
    [NodeGraphDescription("CoinbasePro connector, for retrieve your ApiKey and ApiSecret go to your CoinbasePro account and create them")]
    public class CoinbaseProConnectorNode : Node
    {
        public CoinbaseProConnectorNode(string id, BlockGraph graph)
           : base(id, graph, typeof(CoinbaseProConnectorNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("apiKey", new NodeParameter(this, "apiKey", typeof(string), true));
            this.InParameters.Add("apiSecret", new NodeParameter(this, "apiSecret", typeof(string), true));
            this.InParameters.Add("passphrase", new NodeParameter(this, "passphrase", typeof(string), true));

            this.OutParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
        }

        [JsonIgnore]
        public CoinbaseProClient Client { get; set; }

        [JsonIgnore]
        //public CoinbaseProSocketClient SocketClient { get; set; }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            this.Client = new CoinbaseProClient(new Config { ApiKey = this.InParameters["apiKey"].GetValue().ToString(), Secret = this.InParameters["apiSecret"].GetValue().ToString(), Passphrase = this.InParameters["passphrase"].GetValue().ToString() });
            //this.SocketClient = new CoinbaseProSocketClient();
            this.Next();
        }

        public override void OnStop()
        {
            this.Client.Dispose();
            //this.SocketClient.Dispose();
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
