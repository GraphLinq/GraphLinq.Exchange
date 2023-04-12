using Bitfinex.Net.Objects;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CoinEx.Net.Objects;

namespace NodeBlock.Plugin.Exchange.Nodes.CoinEx.Events
{
    [NodeDefinition("OnCoinExTickerUpdateNode", "On CoinEx Pair Ticker Update", NodeTypeEnum.Event, "CoinEx")]
    [NodeGraphDescription("Get a event when the kline is updated on coinEx for a symbol. Symbol example : ETHBTC")]
    [NodeCycleLimit(1000)]
    public class OnCoinExTickerUpdateNode : Node
    {
        public OnCoinExTickerUpdateNode(string id, BlockGraph graph)
              : base(id, graph, typeof(OnCoinExTickerUpdateNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("coinEx", new NodeParameter(this, "coinEx", typeof(CoinExConnectorNode), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));


            this.OutParameters.Add("amount", new NodeParameter(this, "amount", typeof(double), false));
            this.OutParameters.Add("open", new NodeParameter(this, "open", typeof(double), false));
            this.OutParameters.Add("close", new NodeParameter(this, "close", typeof(double), false));
            this.OutParameters.Add("low", new NodeParameter(this, "low", typeof(double), false));
            this.OutParameters.Add("high", new NodeParameter(this, "high", typeof(double), false));
            this.OutParameters.Add("volume", new NodeParameter(this, "volume", typeof(double), false));

        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            CoinExConnectorNode coinExConnector = this.InParameters["coinEx"].GetValue() as CoinExConnectorNode;

            coinExConnector.SocketClient.SubscribeToKlineUpdates(this.InParameters["symbol"].GetValue().ToString(),KlineInterval.FiveMinute, (currency,data) =>
            {
                
                var instanciatedParameters = this.InstanciatedParametersForCycle();
                
                instanciatedParameters["amount"].SetValue((double)data.First().Amount);
                instanciatedParameters["open"].SetValue((double)data.First().Open);
                instanciatedParameters["close"].SetValue((double)data.First().Close);
                instanciatedParameters["low"].SetValue((double)data.First().Low);
                instanciatedParameters["high"].SetValue((double)data.First().High);
                instanciatedParameters["volume"].SetValue((double)data.First().Volume);
                this.Graph.AddCycle(this, instanciatedParameters);
            });


        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
