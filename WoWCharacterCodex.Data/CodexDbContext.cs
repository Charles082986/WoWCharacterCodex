using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWCharacterCodex.Data
{
    public class CodexDbContext : DbContext
    {
        public CodexDbContext(string connectionString) : base(connectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>().HasKey(c => new { c.Name, c.Realm });
            modelBuilder.Entity<WoWClass>().HasKey(wc => wc.Name);
            modelBuilder.Entity<Specialization>().HasKey(s => new { s.Name, s.WoWClass });
            modelBuilder.Entity<CharacterSpecializationInfo>().HasKey(csi => new { csi.Character, csi.Specialization });
            modelBuilder.Entity<Guild>().HasKey(g => new { g.Name, g.Realm });
            modelBuilder.Entity<GuildMember>().HasKey(g => new { g.Name, g.Realm });
            modelBuilder.Entity<BattlePet>().HasKey(bp => bp.BlizzardId);
            modelBuilder.Entity<Achievement>().HasKey(a => a.BlizzardId);
            modelBuilder.Entity<CharacterBattlePet>().HasKey(cbp => new { cbp.Character, cbp.BattlePet });
            modelBuilder.Entity<CharacterAchievement>().HasKey(ca => new { ca.Character, ca.Achievement });

            modelBuilder.Entity<Character>().HasMany<CharacterSpecializationInfo>(c => c.SpecializationInfos).WithRequired(csi => csi.Character);
            modelBuilder.Entity<Character>().HasRequired<WoWClass>(c => c.Class);
            modelBuilder.Entity<WoWClass>().HasMany<Specialization>(wc => wc.Specializations).WithRequired(s => s.WoWClass);
            modelBuilder.Entity<CharacterSpecializationInfo>().HasRequired<Specialization>(csi => csi.Specialization);
            modelBuilder.Entity<Guild>().HasMany<GuildMember>(g => g.Members).WithOptional(gm => gm.Guild);
            modelBuilder.Entity<Character>().HasMany<CharacterBattlePet>(c => c.BattlePets).WithRequired(cbp => cbp.Character);
            modelBuilder.Entity<CharacterBattlePet>().HasRequired(cbp => cbp.BattlePet);
            modelBuilder.Entity<Character>().HasMany<CharacterAchievement>(c => c.Achievements).WithRequired(ca => ca.Character);
            modelBuilder.Entity<CharacterAchievement>().HasRequired(ca => ca.Achievement);
        }

        public DbSet<Guild> Guilds { get; set; }
        public DbSet<GuildMember> GuildMembers { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<CharacterSpecializationInfo> ChracterSpecializationInfos { get; set; }
        public DbSet<WoWClass> Classes { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<CharacterAchievement> CharacterAchievements { get; set; }
        public DbSet<CharacterBattlePet> CharacterBattlePets { get; set; }
        public DbSet<BattlePet> BattlePets { get; set; }
    }
}
