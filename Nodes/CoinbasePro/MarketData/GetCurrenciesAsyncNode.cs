using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.MarketData
{
    [NodeDefinition("GetCurrenciesAsyncNode", "Get Coinbase Pro Currencies", NodeTypeEnum.Function, "Coinbase Pro Market Data")]
    [NodeGraphDescription("This node lists all known currencies on Coinbase Pro.")]
    public class GetCurrenciesAsyncNode : Node
    {
        public GetCurrenciesAsyncNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetCurrenciesAsyncNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));

            this.OutParameters.Add("currencies", new NodeParameter(this, "currencies", typeof(List<object>), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;

            var currencies = await coinbaseProConnector.Client.MarketData.GetCurrenciesAsync();

            this.OutParameters["currencies"].SetValue(currencies);
            return true;
        }
    }
}
