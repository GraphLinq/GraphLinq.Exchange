using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.KuCoin.Trade
{
    [NodeDefinition("CancelKuCoinOpenOrderNode", "Cancel KuCoin Open Order", NodeTypeEnum.Function, "KuCoin")]
    [NodeGraphDescription("Cancel KuCoin Open Order")]
    public class CancelKuCoinOrderNode : Node
    {
        public CancelKuCoinOrderNode(string id, BlockGraph graph)
            : base(id, graph, typeof(CancelKuCoinOrderNode).Name)
        {
            this.InParameters.Add("kucoin", new NodeParameter(this, "kucoin", typeof(object), true));
            this.InParameters.Add("tradeId", new NodeParameter(this, "tradeId", typeof(string), true));

            this.OutParameters.Add("isSuccess", new NodeParameter(this, "isSuccess", typeof(bool), false));
            this.OutParameters.Add("error", new NodeParameter(this, "error", typeof(string), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            KuCoinConnectorNode connector = this.InParameters["kucoin"].GetValue() as KuCoinConnectorNode;
            var tradeId = this.InParameters["tradeId"].GetValue().ToString();

            var result = connector.Client.CancelOrderAsync(tradeId);

            if (!result.Result.Success)
            {
                this.OutParameters["isSuccess"].SetValue(bool.Parse("false"));
                this.OutParameters["error"].SetValue(result.Result.Error);

                return true;
            }

            this.OutParameters["isSuccess"].SetValue(bool.Parse("true"));
            this.OutParameters["error"].SetValue(result.Result.Error);

            return true;
        }
    }
}
