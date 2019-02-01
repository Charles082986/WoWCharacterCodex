using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WoWCharacterCodex.Application.Blizzard
{
    public class Guild
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("realm")]
        public string Realm { get; set; }
        [JsonProperty("battlegroup")]
        public string Battlegroup { get; set; }
        [JsonProperty("level")]
        public int Level { get; set; }
        [JsonProperty("side")]
        public bool Faction { get; set; }
        [JsonProperty("achievementpoints")]
        public int AchievementPoints { get; set; }
        [JsonProperty("members")]
        public GuildMember[] Members { get; set; }
    }
}
