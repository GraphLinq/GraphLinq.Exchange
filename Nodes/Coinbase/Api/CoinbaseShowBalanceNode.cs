using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Alex75.CoinbaseApiClient;
using Alex75.Cryptocurrencies;
using Microsoft.Extensions.Configuration;


namespace NodeBlock.Plugin.Exchange.Nodes.Coinbase.Api
{
    internal class CoinbaseShowBalanceNode
    {
        /*
        private static void ShowBalance(IClient client)
        {
            Console.WriteLine("\n\n# Account Balance #\n");

            try
            {
                var balance = client.GetBalance().WithoutEmpty();

                Console.WriteLine("Currency | Owned           | Available        ");
                Console.WriteLine("-------- | --------------- | -----------------");
                foreach (var item in balance)
                    Console.WriteLine($" {item.Currency,-7} | {item.OwnedAmount,+15:n12} | {item.AvailableAmount,+15:n12} ");
            }
            catch (Exception exc)
            {
                Console.WriteLine("Failed to GetBalance. " + exc);
            }

            
            ShowTickers(client, new string[] { "ADA/USD", "ETH/USD", "XTZ/USD", "ATOM/USD", "SOL/USD" });

            CreateOrder(client);

            ShowBalance(client);

            ShowOpenOrders(client);
            
        }
        */  
    }
}
