using Coinbase;
using Newtonsoft.Json;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;

namespace NodeBlock.Plugin.Exchange.Nodes.Coinbase
{
    [NodeDefinition("CoinbaseConnectorNode", "Coinbase Connector", NodeTypeEnum.Connector, "Coinbase")]
    [NodeGraphDescription("Coinbase connector, for retrieve your ApiKey and ApiSecret go to your Coinbase account and create them")]
    [NodeSpecialActionAttribute("Go to Coinbase", "open_url", "https://www.coinbase.com")]
    public class CoinbaseConnectorNode : Node
    {
        public CoinbaseConnectorNode(string id, BlockGraph graph)
           : base(id, graph, typeof(CoinbaseConnectorNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("apiKey", new NodeParameter(this, "apiKey", typeof(string), true));
            this.InParameters.Add("apiSecret", new NodeParameter(this, "apiSecret", typeof(string), true));

            this.OutParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseConnectorNode), true));
        }

        [JsonIgnore]
        public CoinbaseClient Client { get; set; }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            var apiKey = this.InParameters["apiKey"].GetValue()?.ToString();
            var apiSecret = this.InParameters["apiSecret"].GetValue()?.ToString();

            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
            {
                this.Client = new CoinbaseClient();
            }
            else
            {
                this.Client = new CoinbaseClient(new ApiKeyConfig
                {
                    ApiKey = apiKey,
                    ApiSecret = apiSecret
                });
            }
            this.Next();
        }

        public override void OnStop()
        {
            this.Client?.Dispose();
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
