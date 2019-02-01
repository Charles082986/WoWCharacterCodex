using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWCharacterCodex.Application.Blizzard
{
    public class WoWClass
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("specializations")]
        public Specialization[] Specializations { get; set; }

    }
}
