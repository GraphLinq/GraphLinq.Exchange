/*
using Coinbase.Pro;
using Coinbase.Pro.Models;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.Reports
{
    [NodeDefinition("CreateReportNode", "Create Coinbase Pro Report", NodeTypeEnum.Function, "Coinbase Pro Reports")]
    [NodeGraphDescription("This node creates a report for trades or account activity on Coinbase Pro.")]
    public class CreateReportNode : Node
    {
        public CreateReportNode(string id, BlockGraph graph)
            : base(id, graph, typeof(CreateReportNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
            this.InParameters.Add("type", new NodeParameter(this, "type", typeof(string), true));
            this.InParameters.Add("startDate", new NodeParameter(this, "startDate", typeof(string), true));
            this.InParameters.Add("endDate", new NodeParameter(this, "endDate", typeof(string), true));
            this.InParameters.Add("productId", new NodeParameter(this, "productId", typeof(string), false));

            this.OutParameters.Add("report", new NodeParameter(this, "report", typeof(Report), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            var type = this.InParameters["type"].GetValue().ToString();
            var startDate = this.InParameters["startDate"].GetValue().ToString();
            var endDate = this.InParameters["endDate"].GetValue().ToString();
            var productId = this.InParameters["productId"].GetValue()?.ToString();

            ReportType reportType = type.Equals("fills", System.StringComparison.OrdinalIgnoreCase) ? ReportType.Fills : ReportType.Account;

            var reportParams = new ReportParams
            {
                Type = reportType,
                StartDate = startDate,
                EndDate = endDate,
                ProductId = productId
            };

            var report = await coinbaseProConnector.Client.Reports.CreateReportAsync(reportParams);

            this.OutParameters["report"].SetValue(report);
            return true;
        }
    }
}
*/
