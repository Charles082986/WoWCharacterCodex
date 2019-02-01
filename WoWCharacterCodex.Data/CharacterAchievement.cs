using System;
using System.Collections.Generic;

namespace WoWCharacterCodex.Data
{
    public class CharacterAchievement
    {
        public virtual Character Character { get; set; }
        public virtual Achievement Achievement { get; set; }

        public DateTime Earned { get; set; }
    }

    public class CharacterAchievementEqualityComparer : IEqualityComparer<CharacterAchievement>
    {
        public bool Equals(CharacterAchievement x, CharacterAchievement y)
        {
            return x.Character.Name == y.Character.Name
                && x.Character.Realm == y.Character.Realm
                && x.Achievement.BlizzardId == y.Achievement.BlizzardId;
        }

        public int GetHashCode(CharacterAchievement obj)
        {
            return obj.GetHashCode();
        }
    }
}