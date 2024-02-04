using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.Conversion
{
    [NodeDefinition("ConvertFundsNode", "Convert Funds in Coinbase Pro", NodeTypeEnum.Function, "Coinbase Pro Conversion")]
    [NodeGraphDescription("This node converts funds from one currency to another within the Coinbase Pro account.")]
    public class ConvertFundsNode : Node
    {
        public ConvertFundsNode(string id, BlockGraph graph)
            : base(id, graph, typeof(ConvertFundsNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
            this.InParameters.Add("fromCurrency", new NodeParameter(this, "fromCurrency", typeof(string), true));
            this.InParameters.Add("toCurrency", new NodeParameter(this, "toCurrency", typeof(string), true));
            this.InParameters.Add("amount", new NodeParameter(this, "amount", typeof(decimal), true));

            this.OutParameters.Add("conversion", new NodeParameter(this, "conversion", typeof(object), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            var conversion = await coinbaseProConnector.Client.Conversion.ConvertAsync(
                this.InParameters["fromCurrency"].GetValue().ToString(),
                this.InParameters["toCurrency"].GetValue().ToString(),
                (decimal)this.InParameters["amount"].GetValue()
            );

            this.OutParameters["conversion"].SetValue(conversion);
            return true;
        }
    }
}
