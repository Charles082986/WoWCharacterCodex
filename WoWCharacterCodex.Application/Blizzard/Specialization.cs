using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWCharacterCodex.Application.Blizzard
{
    public class Specialization
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("role")]
        public Role Role { get; set; }
    }
}
