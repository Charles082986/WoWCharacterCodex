using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWCharacterCodex.Application.Blizzard
{
    public class Raid
    {
        public string Name { get; set; }
        public int LFR { get; set; }
        public int Normal { get; set; }
        public int Heroic { get; set; }
        public int Mythic { get; set; }
        public int Id { get; set; }
        public Boss[] Bosses { get; set; }
    }
}
