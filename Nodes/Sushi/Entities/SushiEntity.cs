using System;
using System.Collections.Generic;
using System.Text;

namespace GraphLinq.SushiAPI.Sushi.Entities
{

    [Serializable]
    public class SushiEntity
    {
        public string Name { get; set; }
        public double SushiDay { get; set; }
        public int AllocPoint { get; set; }
        public double RoiYear { get; set; }
        public double RoiMonth { get; set; }
        public double RoiDay { get; set; }
    }
}
