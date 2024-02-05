using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.MarketData
{
    [NodeDefinition("GetHistoricRatesAsyncNode", "Get Coinbase Pro Historic Rates", NodeTypeEnum.Function, "Coinbase Pro Market Data")]
    [NodeGraphDescription("This node retrieves historic rates for a specified product on Coinbase Pro.")]
    public class GetHistoricRatesAsyncNode : Node
    {
        public GetHistoricRatesAsyncNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetHistoricRatesAsyncNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
            this.InParameters.Add("productId", new NodeParameter(this, "productId", typeof(string), true));
            this.InParameters.Add("start", new NodeParameter(this, "start", typeof(DateTime), false));
            this.InParameters.Add("end", new NodeParameter(this, "end", typeof(DateTime), false));
            this.InParameters.Add("granularity", new NodeParameter(this, "granularity", typeof(int), false));

            this.OutParameters.Add("historicRates", new NodeParameter(this, "historicRates", typeof(object), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            var productId = this.InParameters["productId"].GetValue().ToString();
            var start = (DateTime)this.InParameters["start"].GetValue();
            var end = (DateTime)this.InParameters["end"].GetValue();
            var granularity = (int)this.InParameters["granularity"].GetValue();

            var historicRates = await coinbaseProConnector.Client.MarketData.GetHistoricRatesAsync(productId, start, end, granularity);

            this.OutParameters["historicRates"].SetValue(historicRates);
            return true;
        }
    }
}
