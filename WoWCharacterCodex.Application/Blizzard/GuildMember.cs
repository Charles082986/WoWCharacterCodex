using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWCharacterCodex.Application.Blizzard
{
    public class GuildMember
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("realm")]
        public string Realm { get; set; }
        [JsonProperty("class")]
        public string Class { get; set; }
        [JsonProperty("rank")]
        public int Rank { get; set; }
    }
}
