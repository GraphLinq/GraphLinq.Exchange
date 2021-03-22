using NetTrader.Indicator;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.Binance
{
    [NodeDefinition("GetBinanceFutureUSDTRSINode", "Get Binance Future USDT RSI", NodeTypeEnum.Function, "Binance")]
    [NodeGraphDescription("Get Binance Future USDT RSI for a symbol")]
    public class GetBinanceFutureUSDTRSINode : Node
    {
        public GetBinanceFutureUSDTRSINode(string id, BlockGraph graph)
                    : base(id, graph, typeof(GetBinanceFutureUSDTRSINode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(BinanceConnectorNode), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));

            this.OutParameters.Add("RSI", new NodeParameter(this, "RSI", typeof(double), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            BinanceConnectorNode binanceConnector = this.InParameters["connection"].GetValue() as BinanceConnectorNode;
            var result = binanceConnector.Client.FuturesUsdt.Market.GetKlines(this.InParameters["symbol"].GetValue().ToString(), global::Binance.Net.Enums.KlineInterval.OneHour);

            RSI rsi = new RSI(30);
            List<Ohlc> ohlcList = new List<Ohlc>();
            foreach(var candle in result.Data)
            {
                ohlcList.Add(new Ohlc()
                {
                    Date = candle.CloseTime,
                    Open = (double)candle.Open,
                    High = (double)candle.High,
                    Low = (double)candle.Low,
                    Close = (double)candle.Close,
                    Volume = (double)candle.QuoteVolume,
                    AdjClose = 1
                });
            }
            rsi.Load(ohlcList);
            var serie = rsi.Calculate();
            this.OutParameters["RSI"].SetValue(serie.RSI.LastOrDefault());
            return true;
        }
    }
}
