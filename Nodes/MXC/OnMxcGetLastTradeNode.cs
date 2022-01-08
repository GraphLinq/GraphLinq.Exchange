using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace NodeBlock.Plugin.Exchange.Nodes.MXC
{

    [NodeDefinition("OnMxcGetLastTradeNode", "On Mxc Get Last Trade Node", NodeTypeEnum.Event, "MXC")]
    [NodeGraphDescription("event that get the last transaction by a symbol. symbol example : ETH_USDT")]
    [NodeSpecialActionAttribute("Go to mxc docs #request : sub.symbol", "open_url", "https://github.com/mxcdevelop/APIDoc/blob/master/websocket/spot/websocket-api.md")]
    [NodeCycleLimit(1000)]
    public class OnMxcGetLastTradeNode : Node
    {
        public OnMxcGetLastTradeNode(string id, BlockGraph graph)
              : base(id, graph, typeof(OnMxcGetLastTradeNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));
            this.OutParameters.Add("trades", new NodeParameter(this, "trades", typeof(List<object>), false));

        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        private Socket client;

        public override void SetupEvent()
        {
            client = IO.Socket("wss://wbs.mexc.com", new IO.Options() { Transports = ImmutableList<string>.Empty.Add("websocket") });
            client.On(Socket.EVENT_CONNECT, () =>
            {
                var dictionary = new Dictionary<string, string>();
                dictionary.Add("symbol", this.InParameters["symbol"].GetValue().ToString());
                client.Emit("sub.symbol", dictionary);
            });

            client.On(Socket.EVENT_ERROR, OnError);

            client.On("push.symbol",OnEvent);

            client.Open();


        }

        private void OnError(dynamic error)
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("symbol", this.InParameters["symbol"].GetValue().ToString());
            client.Emit("unsub.symbol", dictionary);
            client.Disconnect();
        }

        private void OnEvent(dynamic data)
        {
            var instanciatedParameters = this.InstanciateParametersForCycle();

            var deals = data.data?.deals;
            if(deals != null)
            {
                instanciatedParameters["trades"].SetValue(deals);

            }
            this.Graph.AddCycle(this, instanciatedParameters);
        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
