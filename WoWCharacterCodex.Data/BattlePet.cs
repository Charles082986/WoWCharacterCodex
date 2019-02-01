using System.Collections.Generic;

namespace WoWCharacterCodex.Data
{
    public class BattlePet
    {
        public int BlizzardId { get; set; }
    }

    public class BattlePetEqualityComparer : IEqualityComparer<BattlePet>
    {
        public bool Equals(BattlePet x, BattlePet y)
        {
            return x.BlizzardId == y.BlizzardId;
        }

        public int GetHashCode(BattlePet obj)
        {
            return obj.GetHashCode();
        }
    }
}