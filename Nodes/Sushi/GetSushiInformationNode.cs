using GraphLinq.SushiAPI.Sushi.Entities;
using Newtonsoft.Json;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.Sushi
{
    [NodeDefinition("GetSushiInformationNode", "Sushi Function", NodeTypeEnum.Function, "Sushi")]
    [NodeGraphDescription("Sushi block for getting information like yield and roi ... Symbol example : COVER-WETH")]
    public  class GetSushiInformationNode : Node
    {
        public GetSushiInformationNode(string id, BlockGraph graph)
      : base(id, graph, typeof(GetSushiInformationNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));


            this.OutParameters.Add("SushiDay", new NodeParameter(this, "SushiDay", typeof(string), true));
            this.OutParameters.Add("AllocPoint", new NodeParameter(this, "AllocPoint", typeof(int), true));
            this.OutParameters.Add("RoiYear", new NodeParameter(this, "RoiYear", typeof(double), true));
            this.OutParameters.Add("RoiMonth", new NodeParameter(this, "RoiMonth", typeof(double), true));
            this.OutParameters.Add("RoiDay", new NodeParameter(this, "RoiDay", typeof(double), true));

        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {

            var packet = new StringContent(JsonConvert.SerializeObject(new
            {
                name = this.InParameters["symbol"].GetValue().ToString()
            }), Encoding.UTF8, "application/json");

           
            var result = false;
            try
            {
                var httpClient = new HttpClient();

                var httpResponse = httpClient.PostAsync("http://localhost:1500/sushi", packet);
                httpResponse.Wait();
                if (httpResponse.Result != null)
                {
                    if (httpResponse.Result.Content != null)
                    {
                        var responseContent = httpResponse.Result.Content.ReadAsStringAsync();
                        responseContent.Wait();
                        var data = JsonConvert.DeserializeObject<SushiEntity>(responseContent.Result);
                        if(data != null)
                        {
                            this.OutParameters["SushiDay"].SetValue(data.SushiDay);
                            this.OutParameters["AllocPoint"].SetValue(data.AllocPoint);
                            this.OutParameters["RoiYear"].SetValue(data.RoiYear);
                            this.OutParameters["RoiMonth"].SetValue(data.RoiMonth);
                            this.OutParameters["RoiDay"].SetValue(data.RoiDay);

                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                        
                    }
                }
            }catch(Exception ex)
            {
                // log the error
            }

            return result;

        }
            
    }
}

