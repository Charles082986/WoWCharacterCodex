﻿namespace WoWCharacterCodex.Application.Blizzard
{
    public class Boss
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LFRKills { get; set; }
        public long LFRTimestamp { get; set; }
        public int NormalKills { get; set; }
        public long NormalTimestamp { get; set; }
        public int HeroicKills { get; set; }
        public long HeroicTimestamp { get; set; }
        public int MythicKills { get; set; }
        public long MythicTimestamp { get; set; }
    }
}