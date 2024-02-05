using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.MarketData
{
    [NodeDefinition("GetTimeAsyncNode", "Get Coinbase Pro Server Time", NodeTypeEnum.Function, "Coinbase Pro Market Data")]
    [NodeGraphDescription("This node retrieves the current server time from Coinbase Pro.")]
    public class GetTimeAsyncNode : Node
    {
        public GetTimeAsyncNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetTimeAsyncNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));

            this.OutParameters.Add("serverTime", new NodeParameter(this, "serverTime", typeof(object), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;

            var serverTime = await coinbaseProConnector.Client.MarketData.GetTimeAsync();

            this.OutParameters["serverTime"].SetValue(serverTime);
            return true;
        }
    }
}
