using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.UserAccount
{
    [NodeDefinition("GetTrailingVolumeNode", "Get Coinbase Pro 30-Day Trailing Volume", NodeTypeEnum.Function, "Coinbase Pro User Account")]
    [NodeGraphDescription("This node retrieves the user's 30-day trailing volume for all products on Coinbase Pro.")]
    public class GetTrailingVolumeNode : Node
    {
        public GetTrailingVolumeNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetTrailingVolumeNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));

            this.OutParameters.Add("trailingVolumes", new NodeParameter(this, "trailingVolumes", typeof(object), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            var trailingVolumes = await coinbaseProConnector.Client.UserAccount.GetTrailingVolumeAsync();

            this.OutParameters["trailingVolumes"].SetValue(trailingVolumes);
            return true;
        }
    }
}
