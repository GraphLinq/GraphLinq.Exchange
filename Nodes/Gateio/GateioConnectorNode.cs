using System;
using System.Collections.Generic;
using System.Text;
using Io.Gate.GateApi.Api;
using Io.Gate.GateApi.Client;
using Io.Gate.GateApi.Model;
using NodeBlock.Engine.Attributes;
using NodeBlock.Engine;
using Newtonsoft.Json;
using System.Diagnostics;

namespace NodeBlock.Plugin.Exchange.Nodes.Gateio
{
    [NodeDefinition("GateioConnectorNode", "Gateio Connector", NodeTypeEnum.Connector, "Gateio")]
    [NodeGraphDescription("Gateio connector")]
    public class GateioConnectorNode : Node
    {
        public GateioConnectorNode(string id, BlockGraph graph)
           : base(id, graph, typeof(GateioConnectorNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("apiKey", new NodeParameter(this, "apiKey", typeof(string), true));
            this.InParameters.Add("apiSecret", new NodeParameter(this, "apiSecret", typeof(string), true));

            this.OutParameters.Add("gateio", new NodeParameter(this, "gateio", typeof(GateioConnectorNode), true));
        }

        [JsonIgnore]
        public ApiClient Client { get; set; }

        //[JsonIgnore]
        //public KrakenSocketClient SocketClient { get; set; }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            //this.Client = new KrakenClient(new KrakenClientOptions()
            //{
            //    ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials(this.InParameters["apiKey"].GetValue().ToString(), this.InParameters["apiSecret"].GetValue().ToString())
            //});
            //this.SocketClient = new KrakenSocketClient();

            Configuration config = new Configuration();
            config.BasePath = "https://api.gateio.ws/api/v4";
            config.ApiV4Key = "735ad9c00e5a20228ba8ab91e8793c52";
            config.ApiV4Secret = "735ad9c00e5a20228ba8ab91e8793c52";
            var apiInstance = new SpotApi(config);
            //var currency = "GT";  // string | Currency name
            //var currencyPair = "BTC_USDT";  // string | Currency pair
            var currencyPair = "BTC_USDT";  // string | Currency pair
            var limit = 100;  // int? | Maximum number of records to be returned in a single list (optional)  (default to 100)
            var lastId = "12345";  // string | Specify list staring point using the `id` of last record in previous list-query results (optional) 
            var reverse = false;  // bool? | Whether the id of records to be retrieved should be less than the last_id specified. Default to false.  When `last_id` is specified. Set `reverse` to `true` to trace back trading history; `false` to retrieve latest tradings.  No effect if `last_id` is not specified. (optional)  (default to false)
            var from = 1627706330;  // long? | Start timestamp of the query (optional) 
            var to = 1635329650;  // long? | Time range ending, default to current time (optional) 
            var page = 1;  // int? | Page number (optional)  (default to 1)

            try
            {
                // Retrieve market trades
                List<Trade> result = apiInstance.ListTrades(currencyPair, limit, lastId, reverse, from, to, page);
                Console.WriteLine(result);
            }
            catch (GateApiException e)
            {
                Console.WriteLine("Exception when calling SpotApi.ListTrades: " + e.Message);
                Console.WriteLine("Exception label: {0}, message: {1}", e.ErrorLabel, e.ErrorMessage);
                Console.WriteLine("Status Code: " + e.ErrorCode);
                Console.WriteLine(e.StackTrace);
            }


            var interval = "\"0\"";  // string | Order depth. 0 means no aggregation is applied. default to 0 (optional)  (default to "0")
            var withId = false;  // bool? | Return order book ID (optional)  (default to false)

            try
            {
                // Retrieve order book
                OrderBook result = apiInstance.ListOrderBook(currencyPair, interval, limit, withId);
                Console.WriteLine(result);
            }
            catch (GateApiException e)
            {
                Console.WriteLine("Exception when calling SpotApi.ListOrderBook: " + e.Message);
                Console.WriteLine("Exception label: {0}, message: {1}", e.ErrorLabel, e.ErrorMessage);
                Console.WriteLine("Status Code: " + e.ErrorCode);


                this.Next();
            }
        }

        public override void OnStop()
        {
            //
        }

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "gateio")
            {
                return this;
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}

