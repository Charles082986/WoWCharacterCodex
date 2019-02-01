using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWCharacterCodex.Data
{
    public class Guild
    {
        public string Name { get; set; }
        public string Realm { get; set; }
        public string Battlegroup { get; set; }
        public int Level { get; set; }
        public bool Side { get; set; }
        public int AchievementPoints { get; set; }
        public DateTime LastRefresh { get; set; }
        public int LastRefreshError { get; set; }
        public virtual ICollection<GuildMember> Members { get; set; }
    }
}
