using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Alex75.CoinbaseApiClient;
using Alex75.Cryptocurrencies;
using Microsoft.Extensions.Configuration;


namespace NodeBlock.Plugin.Exchange.Nodes.Coinbase.Api
{
    internal class CoinbaseCreateOrderNode
    {
        /*
        private static void CreateOrder(IClient client)
        {
            var pair = CurrencyPair.BTC_GBP;
            var ticker = client.GetTicker(pair);
            var amountToSpend = 5; //GBP
            var pricePercentageChange = 1; // 1% less

            var limitPrice = ticker.Bid * ((100 - pricePercentageChange) / 100m);
            var quantity = amountToSpend / limitPrice;

            var orderReference = client.CreateLimitOrder(CreateOrderRequest.Limit(OrderSide.Sell, pair, quantity, limitPrice));
            Console.WriteLine($"Pair: {pair} Ticker Bid: {ticker.Bid} - Order, Quantity: {quantity} Limit price: {limitPrice} Reference: " + orderReference);
        }

        
            ShowTickers(client, new string[] { "ADA/USD", "ETH/USD", "XTZ/USD", "ATOM/USD", "SOL/USD" });

            CreateOrder(client);

            ShowBalance(client);

            ShowOpenOrders(client);
        */
    }
}
