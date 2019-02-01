using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWCharacterCodex.Data
{
    public class CodexRepository
    {
        private CodexDbContext _ctx;
        public CodexRepository(string connectionString)
        {
            _ctx = new CodexDbContext(connectionString);
        }

        public List<Guild> GetGuilds()
        {
            return _ctx.Guilds.Include("Members").ToList();
        }

        public Guild GetGuild(string guildName, string realmName)
        {
            return _ctx.Guilds.Include("Members").FirstOrDefault(g => g.Name == guildName && g.Realm == realmName);
        }

        public List<GuildMember> GetUnguildedMembers()
        {
            return _ctx.GuildMembers.Include("Guild").Where(gm => gm.Guild == null).ToList();
        }

        public List<GuildMember> GetUninitializedGuildMembers()
        {
            var characters = _ctx.Characters.ToList();
            return _ctx.GuildMembers.GroupJoin(
                _ctx.Characters, 
                gm => new { gm.Name, gm.Realm }, 
                c => new { c.Name, c.Realm }, 
                (gm, c) => new { GuildMember = gm, Character = c.FirstOrDefault() })
                .Where(a => a.Character == null)
                .Select(a => a.GuildMember).ToList();
        }

        public Guild Create(Guild guild)
        {
            using (var trans = _ctx.Database.BeginTransaction())
            {
                try
                {
                    _ctx.Guilds.Add(guild);
                    foreach (GuildMember member in guild.Members)
                    {
                        member.Guild = guild;
                        _ctx.Entry(member).State = _ctx.GuildMembers.Contains(member, new GuildMemberEqualityComparer()) ? EntityState.Modified : EntityState.Added;
                    }
                    _ctx.SaveChanges();
                    trans.Commit();
                    return GetGuild(guild.Name, guild.Realm);
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public Guild Update(Guild guild)
        {
            using (var trans = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var ctxGuild = _ctx.Entry(guild);
                    ctxGuild.State = EntityState.Modified;

                    SyncCollections<Guild, GuildMember, GuildMemberEqualityComparer>(guild.Members, ref ctxGuild, "Members", false);
                    
                    _ctx.SaveChanges();
                    trans.Commit();
                    return GetGuild(guild.Name, guild.Realm);
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public List<BattlePet> GetBattlePets()
        {
            return _ctx.BattlePets.ToList();
        }

        public BattlePet GetBattlePet(int blizzardId)
        {
            return _ctx.BattlePets.Find(blizzardId);
        }

        public BattlePet Create(BattlePet battlePet)
        {

            using (var trans = _ctx.Database.BeginTransaction())
            {
                try
                {
                    _ctx.BattlePets.Add(battlePet);
                    _ctx.SaveChanges();
                    trans.Commit();
                    return GetBattlePet(battlePet.BlizzardId);
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public BattlePet Update(BattlePet battlePet)
        {
            using (var trans = _ctx.Database.BeginTransaction())
            {
                try
                {
                    _ctx.Entry(battlePet).State = EntityState.Modified;
                    _ctx.SaveChanges();
                    trans.Commit();
                    return GetBattlePet(battlePet.BlizzardId);
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public List<Achievement> GetAchievements()
        {
            return _ctx.Achievements.ToList();
        }

        public Achievement GetAchievement(int blizzardId)
        {
            return _ctx.Achievements.Find(blizzardId);
        }

        public Achievement Create(Achievement achievement)
        {
            using (var trans = _ctx.Database.BeginTransaction())
            {
                try
                {
                    _ctx.Achievements.Add(achievement);
                    _ctx.SaveChanges();
                    trans.Commit();
                    return GetAchievement(achievement.BlizzardId);
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public Achievement Update(Achievement achievement)
        {
            using (var trans = _ctx.Database.BeginTransaction())
            {
                try
                {
                    _ctx.Entry(achievement).State = EntityState.Modified;
                    _ctx.SaveChanges();
                    trans.Commit();
                    return GetAchievement(achievement.BlizzardId);
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public List<Character> GetCharacters()
        {
            return _ctx.Characters.Include(c => c.Achievements).Include(c => c.BattlePets).Include(c => c.Class).Include(c => c.SpecializationInfos).ToList();
        }

        public Character GetCharacter(string name, string realm)
        {
            return _ctx.Characters
                .Include(c => c.Achievements)
                .Include(c => c.BattlePets)
                .Include(c => c.Class)
                .Include(c => c.SpecializationInfos)
                .FirstOrDefault(c => c.Name == name && c.Realm == realm);
        }

        public Character Create(Character character)
        {
            using (var trans = _ctx.Database.BeginTransaction())
            {
                try
                {
                    character.LastRefresh = DateTime.Now;
                    var savedCharacter = _ctx.Entry(character);
                    savedCharacter.State = EntityState.Added;
                    SyncCollections<Character, CharacterAchievement, CharacterAchievementEqualityComparer>(character.Achievements, ref savedCharacter, "Achievements");
                    SyncCollections<Character, CharacterBattlePet, CharacterBattlePetEqualityComparer>(character.BattlePets, ref savedCharacter, "BattlePets");
                    SyncCollections<Character, CharacterSpecializationInfo, CharacterSpecializationInfoEqualityComparer>(character.SpecializationInfos, ref savedCharacter, "SpecializationInfos");

                    _ctx.SaveChanges();
                    trans.Commit();
                    return GetCharacter(character.Name, character.Realm);
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public Character Update(Character character, bool isRefreshFromBlizzard)
        {
            if (isRefreshFromBlizzard)
            {
                character.LastRefresh = DateTime.Now;
            }
            else
            {
                character.LastUpdate = DateTime.Now;
            }
            using (var trans = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var savedCharacter = _ctx.Entry(character);
                    savedCharacter.State = EntityState.Modified;

                    SyncCollections<Character, CharacterAchievement, CharacterAchievementEqualityComparer>(character.Achievements, ref savedCharacter, "Achievements");
                    SyncCollections<Character, CharacterBattlePet, CharacterBattlePetEqualityComparer>(character.BattlePets, ref savedCharacter, "BattlePets");
                    SyncCollections<Character, CharacterSpecializationInfo, CharacterSpecializationInfoEqualityComparer>(character.SpecializationInfos, ref savedCharacter, "SpecializationInfos");

                    _ctx.SaveChanges();
                    trans.Commit();
                    return GetCharacter(character.Name, character.Realm);
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public Character Save(Character character, bool isRefreshFromBlizzard = false)
        {
            if(GetCharacter(character.Name,character.Realm) != null)
            {
                return Update(character, isRefreshFromBlizzard);
            }
            else
            {
                return Create(character);
            }
        }

        public List<WoWClass> GetClasses()
        {
            return _ctx.Classes.Include(c => c.Specializations).ToList();
        }

        public WoWClass GetWoWClass(string name)
        {
            return _ctx.Classes.Find(name);
        }

        public WoWClass GetWoWClass(int blizzardId)
        {
            return _ctx.Classes.FirstOrDefault(c => c.BlizzardId == blizzardId);
        }

        public WoWClass Create(WoWClass wowClass)
        {
            using (var trans = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var cls = _ctx.Entry(wowClass);
                    cls.State = EntityState.Added;

                    SyncCollections<WoWClass, Specialization, SpecializationEqualityComparer>(wowClass.Specializations, ref cls, "Specializations", false);

                    _ctx.SaveChanges();
                    trans.Commit();
                    return GetWoWClass(wowClass.Name);
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public WoWClass Update(WoWClass wowClass)
        {
            using (var trans = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var savedEntity = _ctx.Entry(wowClass);
                    savedEntity.State = EntityState.Modified;

                    SyncCollections<WoWClass, Specialization, SpecializationEqualityComparer>(wowClass.Specializations, ref savedEntity, "Specializations", false);

                    _ctx.SaveChanges();
                    trans.Commit();
                    return GetWoWClass(wowClass.Name);
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        private void SyncCollections<S,T,K>(ICollection<T> collectionToSave, ref DbEntityEntry<S> savedEntity, string collectionPropertyName, bool deleteRemovedItems = true) where T : class where K : IEqualityComparer<T>, new() where S : class
        {
            if (!savedEntity.Collection<T>(collectionPropertyName).IsLoaded)
            {
                savedEntity.Collection<T>(collectionPropertyName).Load();
            }
            var itemsToAdd = collectionToSave.Except(savedEntity.Collection<T>(collectionPropertyName).CurrentValue, new K());
            var itemsToRemove = savedEntity.Collection<T>(collectionPropertyName).CurrentValue.Except(collectionToSave, new K());
            var itemsToUpdate = collectionToSave.Intersect(savedEntity.Collection<T>(collectionPropertyName).CurrentValue, new K());
            foreach (T entityToSave in itemsToAdd)
            {
                _ctx.Entry(entityToSave).State = _ctx.Set<T>().Contains(entityToSave, new K()) ? EntityState.Modified : EntityState.Added;
                savedEntity.Collection<T>(collectionPropertyName).CurrentValue.Add(entityToSave);
            }
            foreach (T entityToRemove in itemsToRemove)
            {
                if (deleteRemovedItems)
                {
                    _ctx.Entry(entityToRemove).State = EntityState.Deleted;
                }
                savedEntity.Collection<T>(collectionPropertyName).CurrentValue.Remove(entityToRemove);
            }
            foreach (T entityToUpdate in itemsToUpdate)
            {
                _ctx.Entry(entityToUpdate).State = EntityState.Modified;
            }
        }
    }
}
