using Binance.Net.Enums;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.Binance
{
    [NodeDefinition("BinancePlaceLimitBuyOrderNode", "Place Limit Buy Order", NodeTypeEnum.Function, "Binance")]
    [NodeGraphDescription("Allows you to place a limit buy order on binance. example result : true for success order and false for fail")]
    public class BinancePlaceLimitBuyOrderNode : Node
    {
        public BinancePlaceLimitBuyOrderNode(string id, BlockGraph graph)
              : base(id, graph, typeof(BinancePlaceLimitBuyOrderNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(BinanceConnectorNode), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));
            this.InParameters.Add("quantity", new NodeParameter(this, "quantity", typeof(decimal), true));
            this.InParameters.Add("price", new NodeParameter(this, "price", typeof(decimal), true));

            this.OutParameters.Add("orderId", new NodeParameter(this, "orderId", typeof(long), false));
            this.OutParameters.Add("result", new NodeParameter(this, "result", typeof(bool), false));

        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            try
            {
                BinanceConnectorNode binanceConnector = this.InParameters["connection"].GetValue() as BinanceConnectorNode;
                decimal quantity = decimal.Parse(this.InParameters["quantity"].GetValue().ToString(), CultureInfo.InvariantCulture);
                decimal price_r = decimal.Parse(this.InParameters["price"].GetValue().ToString(), CultureInfo.InvariantCulture);

                var order = binanceConnector.Client.Spot.Order.PlaceOrder(this.InParameters["symbol"].GetValue().ToString(), global::Binance.Net.Enums.OrderSide.Buy, global::Binance.Net.Enums.OrderType.Limit, quantity: quantity,timeInForce:TimeInForce.GoodTillCancel,price: price_r);
                this.OutParameters["orderId"].SetValue(order.Data.OrderId);
                this.OutParameters["result"].SetValue(order.Success);

            }
            catch (Exception)
            {
                this.InParameters["result"].SetValue(false);
                return false;
            }

            return true;
        }
    }
}
