using Binance.Net;
using Binance.Net.Objects.Spot;
using Newtonsoft.Json;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using NodeBlock.Plugin.Exchange.Nodes.MXC.API;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.MXC
{
    [NodeDefinition("MxcConnectorNode", "MXC Connector", NodeTypeEnum.Connector, "MXC")]
    [NodeGraphDescription("MXC connector, for retrieve your ApiKey and ApiSecret go to your Binance account and create them")]
    public class MxcConnectorNode : Node
    {
        public MxcConnectorNode(string id, BlockGraph graph)
           : base(id, graph, typeof(MxcConnectorNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("apiKey", new NodeParameter(this, "apiKey", typeof(string), true));
            this.InParameters.Add("apiSecret", new NodeParameter(this, "apiSecret", typeof(string), true));

            this.OutParameters.Add("connection", new NodeParameter(this, "connection", typeof(MxcConnectorNode), true));
        }

        [JsonIgnore]
        public ResetClientWrapper Client { get; set; }



        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            Client = new ResetClientWrapper(this.InParameters["apiSecret"].GetValue().ToString(), this.InParameters["apiKey"].GetValue().ToString());
            this.Next();
        }

        public override void OnStop()
        {
            this.Client.Dispose();
        }

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "connection")
            {
                return this;
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
