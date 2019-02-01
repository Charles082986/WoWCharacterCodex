using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWCharacterCodex.Data
{
    public class CharacterSpecializationInfo
    {
        public virtual Character Character { get; set; }
        public virtual Specialization Specialization { get; set; }
        public int ItemLevel { get; set; }
    }

    public class CharacterSpecializationInfoEqualityComparer : IEqualityComparer<CharacterSpecializationInfo>
    {
        public bool Equals(CharacterSpecializationInfo x, CharacterSpecializationInfo y)
        {
            return x.Character.Name == y.Character.Name
                && x.Character.Realm == y.Character.Realm
                && x.Specialization.WoWClass.Name == y.Specialization.WoWClass.Name
                && x.Specialization.Name == y.Specialization.Name;
        }

        public int GetHashCode(CharacterSpecializationInfo obj)
        {
            return obj.GetHashCode();
        }
    }
}
