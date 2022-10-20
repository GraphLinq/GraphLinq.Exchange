using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Alex75.CoinbaseApiClient;
using Microsoft.Extensions.Configuration;
using Alex75.Cryptocurrencies;

namespace NodeBlock.Plugin.Exchange.Nodes.Coinbase.Api
{
    [NodeDefinition("CoinbaseGetPairPriceNode", "Coinbase Get Pair Price", NodeTypeEnum.Function, "Coinbase")]
    [NodeGraphDescription("Get a ticker pair price from Coinbase")]
    public class CoinbaseGetPairPriceNode : Node
    {
        public CoinbaseGetPairPriceNode(string id, BlockGraph graph)
          : base(id, graph, typeof(CoinbaseGetPairPriceNode).Name)
        {
            //this.InParameters.Add("coinbase", new NodeParameter(this, "coinbase", typeof(CoinbaseConnectorNode), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));


            this.OutParameters.Add("averagePrice", new NodeParameter(this, "averagePrice", typeof(double), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            //CoinbaseConnectorNode coinbaseConnector = this.InParameters["coinbase"].GetValue() as CoinbaseConnectorNode;
            //BitfinexConnectorNode bitfinexConnector = this.InParameters["bitfinex"].GetValue() as BitfinexConnectorNode;
            var symbol = this.InParameters["symbol"].GetValue().ToString();
            //Console.WriteLine(coinbaseConnector);
            //var result = coinbaseConnector.Client.
            var configuration = (new ConfigurationBuilder()).AddUserSecrets("Alex75.CoinbaseApiClient-901e5430-da98-4d0c-a19a-27f7eece8e14").Build();

            var publicKey = configuration["key:tg0Ah6VtGJwS16A6"];
            var secretKey = configuration["key:Vp2tPRRhEMXoRFomMZgFpIjdjhx8okWR"];
            var passphrase = configuration["key:bbe6982d80049a182eb55307b58db84582f354123942c454d1f9c1282b30c9a6"];
            Console.WriteLine("We got here 1");
            //Client = new Alex75.CoinbaseApiClient.Client(publicKey, secretKey, passphrase);
            var client = new Alex75.CoinbaseApiClient.Client(publicKey, secretKey, passphrase);

            ShowTickers(client, new string[] { "ADA/USD", "ETH/USD", "XTZ/USD", "ATOM/USD", "SOL/USD" });

            this.OutParameters["averagePrice"].SetValue(1.123);
            return true;
        }

        private static void ShowTickers(IClient client, string[] pairs)
        {
            Console.WriteLine("\n\n# Tickers #\n");

            Console.WriteLine("Pair       | Buy         | Sell       ");
            Console.WriteLine("---------- | ----------- | -----------");

            var supportedPairs = client.ListPairs();

            foreach (var pair in pairs.Select(pair => CurrencyPair.ParseSlashed(pair)))
            {
                try
                {
                    if (supportedPairs.Contains(pair))
                    {
                        var ticker = client.GetTicker(pair);
                        Console.WriteLine($" {pair.Slashed,-9} | {ticker.Bid,+11} | {ticker.Ask,+11} ");
                    }
                    else
                        Console.WriteLine($" {pair.Slashed,-9} | <not supported> ");
                }
                catch (Exception exc)
                {
                    Console.WriteLine($"Failed to get Ticker for {pair}. " + exc);
                }
            }
        }   
    }
}
