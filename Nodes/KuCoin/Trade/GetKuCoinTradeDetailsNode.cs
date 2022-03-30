using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.KuCoin.Trade
{
    [NodeDefinition("GetKuCoinTradeDetailsNode", "Get KuCoin Trade Details", NodeTypeEnum.Function, "KuCoin")]
    [NodeGraphDescription("Get trade details of a KuCoin order")]
    public class GetKuCoinTradeDetailsNode : Node
    {
        public GetKuCoinTradeDetailsNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetKuCoinTradeDetailsNode).Name)
        {
            this.InParameters.Add("kucoin", new NodeParameter(this, "kucoin", typeof(object), true));
            this.InParameters.Add("tradeId", new NodeParameter(this, "tradeId", typeof(string), true));

            this.OutParameters.Add("isActive", new NodeParameter(this, "isActive", typeof(bool), false));
            this.OutParameters.Add("dealFunds", new NodeParameter(this, "dealFunds", typeof(decimal), false));
            this.OutParameters.Add("dealQuantity", new NodeParameter(this, "dealQuantity", typeof(decimal), false));
            this.OutParameters.Add("fee", new NodeParameter(this, "fee", typeof(decimal), false));
            this.OutParameters.Add("feeCurrency", new NodeParameter(this, "feeCurrency", typeof(string), false));
            this.OutParameters.Add("funds", new NodeParameter(this, "funds", typeof(decimal), false));
            this.OutParameters.Add("price", new NodeParameter(this, "price", typeof(decimal), false));
            this.OutParameters.Add("quantity", new NodeParameter(this, "quantity", typeof(decimal), false));
            this.OutParameters.Add("side", new NodeParameter(this, "side", typeof(string), false));
            this.OutParameters.Add("type", new NodeParameter(this, "type", typeof(string), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            KuCoinConnectorNode connector = this.InParameters["kucoin"].GetValue() as KuCoinConnectorNode;
            var tradeId = this.InParameters["tradeId"].GetValue().ToString();

            var result = connector.Client.GetOrderAsync(tradeId);

            this.OutParameters["isActive"].SetValue(result.Result.Data.IsActive);
            this.OutParameters["dealFunds"].SetValue(result.Result.Data.DealFunds);
            this.OutParameters["dealQuantity"].SetValue(result.Result.Data.DealQuantity);
            this.OutParameters["fee"].SetValue(result.Result.Data.Fee);
            this.OutParameters["feeCurrency"].SetValue(result.Result.Data.FeeCurrency);
            this.OutParameters["funds"].SetValue(result.Result.Data.Funds);
            this.OutParameters["price"].SetValue(result.Result.Data.Price);
            this.OutParameters["quantity"].SetValue(result.Result.Data.Quantity);
            this.OutParameters["side"].SetValue((result.Result.Data.Side == 0) ? "Buy" : "Sell");
            this.OutParameters["type"].SetValue((result.Result.Data.Type == 0) ? "Limit" : "Market");

            return true;
        }
    }
}
