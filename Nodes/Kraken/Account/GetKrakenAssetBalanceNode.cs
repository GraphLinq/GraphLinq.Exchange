using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.Kraken.Account
{
    [NodeDefinition("GetKrakenAssetBalanceNode", "Get Kraken Asset Balance", NodeTypeEnum.Function, "Kraken")]
    [NodeGraphDescription("Get your balance of a specific asset on Kraken")]
    public class GetKrakenAssetBalanceNode : Node 
    {
        public GetKrakenAssetBalanceNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetKrakenAssetBalanceNode).Name)
        {
            this.InParameters.Add("kraken", new NodeParameter(this, "kraken", typeof(object), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));

            this.OutParameters.Add("balance", new NodeParameter(this, "balance", typeof(double), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            KrakenConnectorNode connector = this.InParameters["kraken"].GetValue() as KrakenConnectorNode;
            var symbol = this.InParameters["symbol"].GetValue().ToString();
            var balances = connector.Client.GetBalances();

            this.OutParameters["balance"].SetValue(balances.Data[symbol]);
            return true;
        }
    }
}
