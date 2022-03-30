using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.KuCoin.Trade
{
    [NodeDefinition("GetKuCoinSingleOrderStatusNode", "Get KuCoin Single Order Status", NodeTypeEnum.Function, "KuCoin")]
    [NodeGraphDescription("Get the status of a single KuCoin order")]
    public class GetKuCoinSingleOrderStatusNode : Node
    {
        public GetKuCoinSingleOrderStatusNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetKuCoinSingleOrderStatusNode).Name)
        {
            this.InParameters.Add("kucoin", new NodeParameter(this, "kucoin", typeof(object), true));
            this.InParameters.Add("tradeId", new NodeParameter(this, "tradeId", typeof(string), true));

            this.OutParameters.Add("isActive", new NodeParameter(this, "isActive", typeof(bool), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            KuCoinConnectorNode connector = this.InParameters["kucoin"].GetValue() as KuCoinConnectorNode;
            var tradeId = this.InParameters["tradeId"].GetValue().ToString();

            var result = connector.Client.GetOrderAsync(tradeId);

            this.OutParameters["isActive"].SetValue(result.Result.Data.IsActive);

            return true;
        }
    }
}
