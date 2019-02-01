using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWCharacterCodex.Data
{
    public class GuildMember
    {
        public virtual Guild Guild { get; set; }
        public string Name { get; set; }
        public string Realm { get; set; }
        public string Class { get; set; }
        public int Rank { get; set; }
    }

    public class GuildMemberEqualityComparer : IEqualityComparer<GuildMember>
    {
        public bool Equals(GuildMember x, GuildMember y)
        {
            return x.Name == y.Name && x.Realm == y.Realm;
        }

        public int GetHashCode(GuildMember obj)
        {
            return obj.GetHashCode();
        }
    }
}
