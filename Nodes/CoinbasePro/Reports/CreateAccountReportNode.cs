/*
using Coinbase.Pro;
using Coinbase.Pro.Models;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinbasePro.Reports
{
    [NodeDefinition("CreateAccountReportNode", "Create Coinbase Pro Account Activity Report", NodeTypeEnum.Function, "Coinbase Pro Reports")]
    [NodeGraphDescription("This node creates a report detailing account activity on Coinbase Pro.")]
    public class CreateAccountReportNode : Node
    {
        public CreateAccountReportNode(string id, BlockGraph graph)
            : base(id, graph, typeof(CreateAccountReportNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseProConnectorNode), true));
            this.InParameters.Add("accountId", new NodeParameter(this, "accountId", typeof(string), true));
            this.InParameters.Add("startDate", new NodeParameter(this, "startDate", typeof(DateTime), true));
            this.InParameters.Add("endDate", new NodeParameter(this, "endDate", typeof(DateTime), true));
            this.InParameters.Add("pdfOrCsv", new NodeParameter(this, "pdfOrCsv", typeof(string), true)); // Accepts "pdf" or "csv"
            this.InParameters.Add("email", new NodeParameter(this, "email", typeof(string), false)); // Optional

            this.OutParameters.Add("report", new NodeParameter(this, "report", typeof(object), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public async Task<bool> OnExecutionAsync()
        {
            CoinbaseProConnectorNode coinbaseProConnector = this.InParameters["connection"].GetValue() as CoinbaseProConnectorNode;
            var accountId = this.InParameters["accountId"].GetValue().ToString();
            var startDate = (DateTime)this.InParameters["startDate"].GetValue();
            var endDate = (DateTime)this.InParameters["endDate"].GetValue();
            var productId = this.InParameters["productId"].GetValue()?.ToString();
            var formatString = this.InParameters["pdfOrCsv"].GetValue().ToString().ToLower();
            var email = this.InParameters["email"].GetValue().ToString();

            ReportFormat format = formatString == "pdf" ? ReportFormat.Pdf : ReportFormat.Csv;

            var reportParams = new CreateReport
            {
                StartDate = startDate,
                EndDate = endDate,
                AccountId = accountId,
                ProductId = productId,
                Format = format,
                Email = email
            };

            var report = await coinbaseProConnector.Client.Reports.CreateAccountReportAsync(reportParams);

            this.OutParameters["report"].SetValue(report);
            return true;
        }
    }
}
*/
