using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWCharacterCodex.Data
{
    public class Character
    {
        public string Name { get; set; }
        public string Realm { get; set; }
        public virtual WoWClass Class { get; set; }        
        public virtual ICollection<CharacterSpecializationInfo> SpecializationInfos { get; set; }
        public virtual ICollection<CharacterAchievement> Achievements { get; set; }
        public virtual ICollection<CharacterBattlePet> BattlePets { get; set; }
        public string AchievementStamp { get; set; }
        public string BattlePetStamp { get; set; }
        public DateTime? LastUpdate { get; set; }
        public DateTime LastRefresh { get; set; }
        public int LastRefreshError { get; set; }
    }
}
