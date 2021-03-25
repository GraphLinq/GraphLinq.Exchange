using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.KuCoin.Account
{
    [NodeDefinition("GetKuCoinAssetBalance", "Get KuCoin Asset Balance", NodeTypeEnum.Function, "KuCoin")]
    [NodeGraphDescription("Get your balance of a specific asset on KuCoin")]
    public class GetKuCoinAssetBalance : Node
    {
        public GetKuCoinAssetBalance(string id, BlockGraph graph)
            : base(id, graph, typeof(GetKuCoinAssetBalance).Name)
        {
            this.InParameters.Add("kucoin", new NodeParameter(this, "kucoin", typeof(object), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));
            this.InParameters.Add("tradeAccount", new NodeParameter(this, "tradeAccount", typeof(bool), true));

            this.OutParameters.Add("balance", new NodeParameter(this, "balance", typeof(double), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            KuCoinConnectorNode connector = this.InParameters["kucoin"].GetValue() as KuCoinConnectorNode;
            var symbol = this.InParameters["symbol"].GetValue().ToString();
            var tradeAccount = bool.Parse(this.InParameters["tradeAccount"].GetValue().ToString());
            var result = connector.Client.GetTransferable(symbol, tradeAccount ? Kucoin.Net.Objects.KucoinAccountType.Trade : Kucoin.Net.Objects.KucoinAccountType.Main);
            this.OutParameters["balance"].SetValue(result.Data.Balance);
            return true;
        }
    }
}
