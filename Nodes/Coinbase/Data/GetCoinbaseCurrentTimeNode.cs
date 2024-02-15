using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using Coinbase.Models;
using Newtonsoft.Json;
using System;

namespace NodeBlock.Plugin.Exchange.Nodes.Coinbase.Data
{
    [NodeDefinition("GetCoinbaseCurrentTimeNode", "Get Coinbase Time", NodeTypeEnum.Function, "Coinbase")]
    [NodeGraphDescription("Get the time on Coinbase")]
    public class GetCoinbaseCurrentTimeNode : Node
    {
        public GetCoinbaseCurrentTimeNode(string id, BlockGraph graph)
              : base(id, graph, typeof(GetCoinbaseCurrentTimeNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(CoinbaseConnectorNode), true));

            this.OutParameters.Add("iso", new NodeParameter(this, "iso", typeof(DateTimeOffset), false));
            this.OutParameters.Add("epoch", new NodeParameter(this, "epoch", typeof(long), false));
        }

        [JsonIgnore]
        public JsonResponse Time { get; private set; }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            CoinbaseConnectorNode coinbaseConnector = this.InParameters["connection"].GetValue() as CoinbaseConnectorNode;

            var request = coinbaseConnector.Client.Data.GetCurrentTimeAsync();
            request.Wait();

            var response = new Time
            {
                Iso = request.Result.Data.Iso,
                Epoch = request.Result.Data.Epoch
            };

            this.OutParameters["iso"].SetValue(response.Iso);
            this.OutParameters["epoch"].SetValue(response.Epoch);

            return true;
        }
    }
}
