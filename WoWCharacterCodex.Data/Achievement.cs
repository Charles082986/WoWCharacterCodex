using System.Collections.Generic;

namespace WoWCharacterCodex.Data
{
    public class Achievement
    {
        public int BlizzardId { get; set; }
        public string Name { get; set; }
    }

    public class AchievementEqualityComparer : IEqualityComparer<Achievement>
    {
        public bool Equals(Achievement x, Achievement y)
        {
            return x.BlizzardId == y.BlizzardId;
        }

        public int GetHashCode(Achievement obj)
        {
            return obj.GetHashCode();
        }
    }
}