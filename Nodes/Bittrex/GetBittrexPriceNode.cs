using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.Bittrex
{
    [NodeDefinition("GetBittrexPriceNode.", "Get Bittrex Price", NodeTypeEnum.Function, "Bittrex")]
    [NodeGraphDescription("Get the current price on bittrex for a symbol. Symbol example : BTC-USDT")]
    public class GetBittrexPriceNode : Node
    {
        public GetBittrexPriceNode(string id, BlockGraph graph)
          : base(id, graph, typeof(GetBittrexPriceNode).Name)
        {
            this.InParameters.Add("bittrex", new NodeParameter(this, "bittrex", typeof(BittrexConnectorNode), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));


            this.OutParameters.Add("askRate", new NodeParameter(this, "askRate", typeof(decimal), false));
            this.OutParameters.Add("bidRate", new NodeParameter(this, "bidRate", typeof(decimal), false));

        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            BittrexConnectorNode bitfinexConnector = this.InParameters["bittrex"].GetValue() as BittrexConnectorNode;
            var symbol = this.InParameters["symbol"].GetValue().ToString();
            var result = bitfinexConnector.Client.GetTickerAsync(symbol);
            result.Wait();
            this.OutParameters["askRate"].SetValue(result.Result.Data.AskRate);
            this.OutParameters["bidRate"].SetValue(result.Result.Data.BidRate);

            return true;
        }
    }
}
