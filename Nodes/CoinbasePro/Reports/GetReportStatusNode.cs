using Coinbase.Pro;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.Reports
{
    [NodeDefinition("GetReportStatusNode", "Get Coinbase Pro Report Status", NodeTypeEnum.Function, "Coinbase Pro Reports")]
    [NodeGraphDescription("This node retrieves the status of a specified report on Coinbase Pro.")]
    public class GetReportStatusNode : Node
    {
        public GetReportStatusNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetReportStatusNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
            this.InParameters.Add("reportId", new NodeParameter(this, "reportId", typeof(string), true));

            this.OutParameters.Add("reportStatus", new NodeParameter(this, "reportStatus", typeof(string), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            var reportId = this.InParameters["reportId"].GetValue().ToString();

            var report = await coinbaseProConnector.Client.Reports.GetReportStatusAsync(reportId);

            this.OutParameters["reportStatus"].SetValue(report.Status.ToString());
            return true;
        }
    }
}
