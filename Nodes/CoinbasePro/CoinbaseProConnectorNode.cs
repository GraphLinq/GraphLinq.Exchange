using Coinbase.Pro;
using Coinbase.Pro.WebSockets;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

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
            this.InParameters.Add("sandbox", new NodeParameter(this, "sandbox", typeof(bool), false));

            this.OutParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
        }

        [JsonIgnore]
        public CoinbaseProClient Client { get; set; }

        [JsonIgnore]
        public CoinbaseProWebSocket SocketClient { get; set; }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            if (this.InParameters["sandbox"].GetValue() is true)
            {
                this.Client = new CoinbaseProClient(new Config
                {
                    ApiKey = this.InParameters["apiKey"].GetValue().ToString(),
                    Secret = this.InParameters["apiSecret"].GetValue().ToString(),
                    Passphrase = this.InParameters["passphrase"].GetValue().ToString(),
                    ApiUrl = "https://api-public.sandbox.pro.coinbase.com"
                });

                this.SocketClient = new CoinbaseProWebSocket(new WebSocketConfig
                {
                    ApiKey = this.InParameters["apiKey"].GetValue().ToString(),
                    Secret = this.InParameters["apiSecret"].GetValue().ToString(),
                    Passphrase = this.InParameters["passphrase"].GetValue().ToString(),
                    SocketUri = "wss://ws-feed-public.sandbox.pro.coinbase.com"
                });
            }
            else
            {
                Console.WriteLine("USE PRODUCTION");
                this.Client = new CoinbaseProClient(new Config
                {
                    ApiKey = this.InParameters["apiKey"].GetValue().ToString(),
                    Secret = this.InParameters["apiSecret"].GetValue().ToString(),
                    Passphrase = this.InParameters["passphrase"].GetValue().ToString()
                });

                this.SocketClient = new CoinbaseProWebSocket(new WebSocketConfig
                {
                    ApiKey = this.InParameters["apiKey"].GetValue().ToString(),
                    Secret = this.InParameters["apiSecret"].GetValue().ToString(),
                    Passphrase = this.InParameters["passphrase"].GetValue().ToString(),
                });
            }

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
