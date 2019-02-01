using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWCharacterCodex.Application.Blizzard
{
    public class Character
    {
        public string Name { get; set; }
        public string Realm { get; set; }
        public string Battlegroup { get; set; }
        public int Class { get; set; }
        public int Race { get; set; }
        public bool Gender { get; set; }
        public int Level { get; set; }
        public int AchievementPoints { get; set; }
        public bool Faction { get; set; }
        public Progression Progression { get; set; }
    }
}
