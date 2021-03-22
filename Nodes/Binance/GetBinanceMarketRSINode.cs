using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NodeBlock.Plugin.Exchange.Nodes.Binance
{
    [NodeDefinition("GetBinanceMarketMomentumNode", "Get Binance Market RSI", NodeTypeEnum.Function, "Binance")]
    [NodeGraphDescription("Get Binance Market RSI for a symbol")]
    public class GetBinanceMarketRSINode : Node
    {
        public GetBinanceMarketRSINode(string id, BlockGraph graph)
                    : base(id, graph, typeof(GetBinanceMarketRSINode).Name)
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
            var result = binanceConnector.Client.Spot.Market.GetKlines(this.InParameters["symbol"].GetValue().ToString(), global::Binance.Net.Enums.KlineInterval.ThirtyMinutes);

            var rsi = this.CalculateRsi(result.Data.Select(x => (double)x.Close).ToList());
            this.OutParameters["RSI"].SetValue(rsi);
            return true;
        }

        public double CalculateRsi(IEnumerable<double> closePrices)
        {
            double Tolerance = 10e-20;
            var prices = closePrices as double[] ?? closePrices.ToArray();

            double sumGain = 0;
            double sumLoss = 0;
            for (int i = 1; i < prices.Length; i++)
            {
                var difference = prices[i] - prices[i - 1];
                if (difference >= 0)
                {
                    sumGain += difference;
                }
                else
                {
                    sumLoss -= difference;
                }
            }

            if (sumGain == 0) return 0;
            if (Math.Abs(sumLoss) < Tolerance) return 100;

            var relativeStrength = sumGain / sumLoss;

            return 100.0 - (100.0 / (1 + relativeStrength));
        }
    }
}
