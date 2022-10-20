using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using Alex75.CoinbaseApiClient;
using Alex75.Cryptocurrencies;
using Microsoft.Extensions.Configuration;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;


namespace NodeBlock.Plugin.Exchange.Nodes.Coinbase.Api
{
    [NodeDefinition("CoinbaseConnectorNode.", "Coinbase Connector Node", NodeTypeEnum.Connector, "Coinbase")]
    [NodeGraphDescription("Connect to the Coinbase API")]

    public class CoinbaseConnectorNode : Node
    {
        public CoinbaseConnectorNode(string id, BlockGraph graph)
          : base(id, graph, typeof(CoinbaseConnectorNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("apiKey", new NodeParameter(this, "apiKey", typeof(string), true));
            this.InParameters.Add("apiSecret", new NodeParameter(this, "apiSecret", typeof(string), true));

            this.OutParameters.Add("coinbase", new NodeParameter(this, "coinbase", typeof(CoinbaseConnectorNode), true));

        }

        [JsonIgnore]
        public Coinbase.Api.CoinbaseConnectorNode Client { get; set; }


        [JsonIgnore]
        //public BittrexSocketClient SocketClient { get; set; }
        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {

            var configuration = (new ConfigurationBuilder()).AddUserSecrets("Alex75.CoinbaseApiClient-901e5430-da98-4d0c-a19a-27f7eece8e14").Build();

            var publicKey = configuration["key:tg0Ah6VtGJwS16A6"];
            var secretKey = configuration["key:Vp2tPRRhEMXoRFomMZgFpIjdjhx8okWR"];
            var passphrase = configuration["key:bbe6982d80049a182eb55307b58db84582f354123942c454d1f9c1282b30c9a6"];
            Console.WriteLine("We got here 1");
            //Client = new Alex75.CoinbaseApiClient.Client(publicKey, secretKey, passphrase);
            var client = new Alex75.CoinbaseApiClient.Client(publicKey, secretKey, passphrase);
            this.Next();
        }


        public override void OnStop()
        {
            //this.client.Dispose();
            //this.SocketClient.Dispose();
        }


        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "coinbase")
            {
                Console.WriteLine("Returned This");
                return this;
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
