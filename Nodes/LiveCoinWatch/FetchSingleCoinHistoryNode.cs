using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;

namespace NodeBlock.Plugin.Exchange.Nodes.LiveCoinWatch
{
    [NodeDefinition("FetchSingleCoinHistoryNode", "Fetch Single Coin History", NodeTypeEnum.Function, "LiveCoinWatch")]
    [NodeGraphDescription("Fetch coin history on LiveCoinWatch")]
    [NodeIDEParameters(Hidden = true)]

    public class FetchSingleCoinHistoryNode : Node
    {

        public DateTime UnixTimeStampToDate(long timestamp)
        {
            DateTime unixDateTime = new DateTime(1970, 1, 1, 0, 0, 0);
            return unixDateTime.AddMilliseconds(timestamp);
        }

        public FetchSingleCoinHistoryNode(string id, BlockGraph graph)
            : base(id, graph, typeof(FetchSingleCoinHistoryNode).Name)
        {
            this.InParameters.Add("liveCoinWatch", new NodeParameter(this, "liveCoinWatch", typeof(LiveCoinWatchConnectorNode), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));
            this.InParameters.Add("currency", new NodeParameter(this, "currency", typeof(string), true));
            this.InParameters.Add("start", new NodeParameter(this, "start", typeof(int), true));
            this.InParameters.Add("end", new NodeParameter(this, "end", typeof(int), true));

            this.OutParameters.Add("fullHistory", new NodeParameter(this, "fullHistory", typeof(string), false));
            this.OutParameters.Add("dates", new NodeParameter(this, "dates", typeof(string), false));
            this.OutParameters.Add("rates", new NodeParameter(this, "rates", typeof(string), false));
            this.OutParameters.Add("volumes", new NodeParameter(this, "volumes", typeof(string), false));
            this.OutParameters.Add("mCaps", new NodeParameter(this, "mCaps", typeof(string), false));
            this.OutParameters.Add("liquidities", new NodeParameter(this, "liquidities", typeof(string), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            LiveCoinWatchConnectorNode liveCoinWatchConnectorNode = this.InParameters["liveCoinWatch"].GetValue() as LiveCoinWatchConnectorNode;

            var coinRequest = liveCoinWatchConnectorNode.API.FetchCoinSingleHistory(
                this.InParameters["currency"].GetValue().ToString(),
                this.InParameters["symbol"].GetValue().ToString(),
                this.InParameters["start"].GetValue(),
                this.InParameters["end"].GetValue()
            );
            coinRequest.Wait();

            List<DateTime> dateLi = coinRequest.Result.History.Select(o => UnixTimeStampToDate(o.Date)).ToList();
            List<string> dateList = dateLi.Select(o => o.ToString("g")).ToList();
            List<double> rateList = coinRequest.Result.History.Select(o => o.Rate).ToList();
            List<long> volumeList = coinRequest.Result.History.Select(o => o.Volume).ToList();
            List<long> mktCapList = coinRequest.Result.History.Select(o => o.Cap).ToList();
            List<long> liquidList = coinRequest.Result.History.Select(o => o.Liquidity).ToList();

            string fullHistory = JsonConvert.SerializeObject(coinRequest.Result.History);

            // All data points returned
            //string dateOfData = JsonConvert.SerializeObject(dateOfList);
            //string ratePrData = JsonConvert.SerializeObject(rateList);
            //string volumeData = JsonConvert.SerializeObject(volumeList);
            //string mktCapData = JsonConvert.SerializeObject(mktCapList);
            //string liquidData = JsonConvert.SerializeObject(liquidList);

            // Every Nth data point returned
            string dateOfData = JsonConvert.SerializeObject(dateList.Where((x, i) => i % 2 == 0));
            string ratePrData = JsonConvert.SerializeObject(rateList.Where((x, i) => i % 2 == 0));
            string volumeData = JsonConvert.SerializeObject(volumeList.Where((x, i) => i % 2 == 0));
            string mktCapData = JsonConvert.SerializeObject(mktCapList.Where((x, i) => i % 2 == 0));
            string liquidData = JsonConvert.SerializeObject(liquidList.Where((x, i) => i % 2 == 0));

            this.OutParameters["fullHistory"].SetValue(fullHistory).ToString();
            this.OutParameters["dates"].SetValue(dateOfData).ToString();
            this.OutParameters["rates"].SetValue(ratePrData.ToString());
            this.OutParameters["volumes"].SetValue(volumeData.ToString());
            this.OutParameters["mCaps"].SetValue(mktCapData).ToString();
            this.OutParameters["liquidities"].SetValue(liquidData).ToString();

            return true;
        }
    }
}
