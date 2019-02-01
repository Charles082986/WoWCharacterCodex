using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WoWCharacterCodex.Data
{
    public class WoWClass
    {
        public int BlizzardId { get; set; }
        public string Name { get; set; }
        public string ArmorType { get; set; }
        public virtual ICollection<Specialization> Specializations { get; set; }
    }
}