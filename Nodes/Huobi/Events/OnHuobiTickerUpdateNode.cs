using Huobi.Net.Objects;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.Huobi.Events
{
    [NodeDefinition("OnHuobiTickerUpdateNode", "On Huobi Pair Kline Update", NodeTypeEnum.Event, "Huobi")]
    [NodeGraphDescription("Get a event when the kline is updated on Huobi for a symbol. Symbol example : ethusdt")]
    [NodeCycleLimit(1000)]
    public class OnHuobiTickerUpdateNode : Node
    {
        public OnHuobiTickerUpdateNode(string id, BlockGraph graph)
            : base(id, graph, typeof(OnHuobiTickerUpdateNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("huobi", new NodeParameter(this, "huobi", typeof(HuobiConnectorNode), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));

            this.OutParameters.Add("amount", new NodeParameter(this, "amount", typeof(double), false));
            this.OutParameters.Add("open", new NodeParameter(this, "open", typeof(double), false));
            this.OutParameters.Add("close", new NodeParameter(this, "close", typeof(double), false));
            this.OutParameters.Add("low", new NodeParameter(this, "low", typeof(double), false));
            this.OutParameters.Add("high", new NodeParameter(this, "high", typeof(double), false));
            this.OutParameters.Add("tradeCount", new NodeParameter(this, "tradeCount", typeof(int), false));
            this.OutParameters.Add("volume", new NodeParameter(this, "volume", typeof(double), false));
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            HuobiConnectorNode connector = this.InParameters["huobi"].GetValue() as HuobiConnectorNode;
            connector.SocketClient.SubscribeToKlineUpdates(this.InParameters["symbol"].GetValue().ToString(), HuobiPeriod.FiveMinutes, (data) =>
            {
                var instanciatedParameters = this.InstanciateParametersForCycle();
                instanciatedParameters["amount"].SetValue((double)data.Amount);
                instanciatedParameters["open"].SetValue((double)data.Open);
                instanciatedParameters["close"].SetValue((double)data.Close);
                instanciatedParameters["low"].SetValue((double)data.Low);
                instanciatedParameters["high"].SetValue((double)data.High);
                instanciatedParameters["tradeCount"].SetValue((int)data.TradeCount);
                instanciatedParameters["volume"].SetValue((double)data.Volume);
                this.Graph.AddCycle(this, instanciatedParameters);
            });
        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
