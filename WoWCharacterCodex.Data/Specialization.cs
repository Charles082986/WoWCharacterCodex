using System.Collections.Generic;

namespace WoWCharacterCodex.Data
{
    public class Specialization
    {
        public virtual WoWClass WoWClass { get; set; }
        public int SpecId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string PrimaryStat { get; set; }
    }

    public class SpecializationEqualityComparer : IEqualityComparer<Specialization>
    {
        public bool Equals(Specialization x, Specialization y)
        {
            return x.Name == y.Name && x.WoWClass.Name == y.WoWClass.Name;
        }

        public int GetHashCode(Specialization obj)
        {
            return obj.GetHashCode();
        }
    }
}