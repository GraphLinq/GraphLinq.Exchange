using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Alex75.CoinbaseApiClient;
using Alex75.Cryptocurrencies;
using Microsoft.Extensions.Configuration;


namespace NodeBlock.Plugin.Exchange.Nodes.Coinbase.Api
{
    internal class CoinbaseShowOpenOrdersNode
    {
        /*
        private static void ShowOpenOrders(IClient client)
        {
            Console.WriteLine("\n\n# Open Orders #\n");

            try
            {
                var orders = client.ListOpenOrders();
                foreach (var order in orders)
                    Console.WriteLine(order.Print());

                if (orders.Length == 0)
                    Console.WriteLine("<none>\n");
            }
            catch (Exception exc)
            {
                Console.WriteLine("Failed to ShowOpenOrders. " + exc);
            }
        }

        
            ShowTickers(client, new string[] { "ADA/USD", "ETH/USD", "XTZ/USD", "ATOM/USD", "SOL/USD" });

            CreateOrder(client);

            ShowBalance(client);

            ShowOpenOrders(client);
        */
    }
}
