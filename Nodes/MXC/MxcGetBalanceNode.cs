using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using NodeBlock.Plugin.Exchange.Nodes.MXC.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.MXC
{
    [NodeDefinition("MxcGetBalanceNode.", "Get MXC Balance", NodeTypeEnum.Function, "MXC")]
    [NodeGraphDescription("Get the current account balance on MXC. data is json result.")]
    public class MxcGetBalanceNode : Node
    {
        public MxcGetBalanceNode(string id, BlockGraph graph)
          : base(id, graph, typeof(MxcGetBalanceNode).Name)
        {
            this.InParameters.Add("mxc", new NodeParameter(this, "mxc", typeof(MxcConnectorNode), true));

            this.OutParameters.Add("data", new NodeParameter(this, "data", typeof(string), false));

        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            MxcConnectorNode connector = this.InParameters["mxc"].GetValue() as MxcConnectorNode;

            var result = connector.Client.Get<BalanceEntity>("/open/api/v2/account/info", new Dictionary<string, string>(),true);

            this.OutParameters["data"].SetValue(result.data.ToString());
            return true;
        }
    }
}
