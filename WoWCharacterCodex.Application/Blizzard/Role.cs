using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWCharacterCodex.Application.Blizzard
{
    public class Role
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
