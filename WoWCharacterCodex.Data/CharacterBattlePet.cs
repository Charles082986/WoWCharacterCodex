using System.Collections.Generic;

namespace WoWCharacterCodex.Data
{
    public class CharacterBattlePet
    {
        public virtual Character Character { get; set; }
        public virtual BattlePet BattlePet { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Level { get; set; }
    }

    public class CharacterBattlePetEqualityComparer : IEqualityComparer<CharacterBattlePet>
    {
        public bool Equals(CharacterBattlePet x, CharacterBattlePet y)
        {
            return x.Character.Name == y.Character.Name
                && x.Character.Realm == y.Character.Realm
                && x.BattlePet.BlizzardId == y.BattlePet.BlizzardId;
        }

        public int GetHashCode(CharacterBattlePet obj)
        {
            return obj.GetHashCode();
        }
    }
}