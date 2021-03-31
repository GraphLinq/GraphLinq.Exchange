using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NodeBlock.Plugin.Exchange.Nodes.Bitfinex.Account
{
    [NodeDefinition("GetBitfinexAssetBalanceNode", "Get Bitfinex Asset Balance", NodeTypeEnum.Function, "Bitfinex")]
    [NodeGraphDescription("Get your balance of a specific asset on Bitfinex")]
    public class GetBitfinexAssetBalanceNode : Node
    {
        public GetBitfinexAssetBalanceNode(string id, BlockGraph graph)
          : base(id, graph, typeof(GetBitfinexAssetBalanceNode).Name)
        {
            this.InParameters.Add("bitfinex", new NodeParameter(this, "bitfinex", typeof(BitfinexConnectorNode), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));

            this.OutParameters.Add("balance", new NodeParameter(this, "balance", typeof(double), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            BitfinexConnectorNode connector = this.InParameters["bitfinex"].GetValue() as BitfinexConnectorNode;
            var symbol = this.InParameters["symbol"].GetValue().ToString();
            var balances = connector.Client.GetBalances();
            var result = balances.Data.FirstOrDefault(x => x.Currency == symbol);
            if(result != null)
            {
                this.OutParameters["balance"].SetValue((double)result.Balance);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
