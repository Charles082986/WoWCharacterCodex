using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoWCharacterCodex.Data;

namespace WoWCharacterCodex.Application
{
    public class CodexService
    {
        CodexRepository _codex;
        CredentialService _credentials;
        BlizzardService _blizzard;
        private static List<WoWClass> _classes;

        public CodexService(string codexConnectionString, string credentialConnectionString)
        {
            _codex = new CodexRepository(codexConnectionString);
            _credentials = new CredentialService(credentialConnectionString);
            _blizzard = new BlizzardService(_credentials);
        }

        public static void MapperConfig()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Blizzard.Guild, Data.Guild>()
                    .ForMember(dest => dest.Side, exp => exp.MapFrom(src => src.Faction));

                cfg.CreateMap<Blizzard.GuildMember, Data.GuildMember>();
            });
        }

        public async Task<IEnumerable<Data.Guild>> GetGuilds()
        {
            return await Task.Run(() => _codex.GetGuilds());
        }

        public async Task RefreshGuildsAsync()
        {
            try
            {
                var guilds = await GetGuilds();
                foreach (Guild g in guilds)
                {
                    Blizzard.Guild blizzardGuild = await Task.Run(() => _blizzard.GetGuild(g.Name, g.Realm));
                    if (blizzardGuild != null)
                    {
                        await Task.Run(() => _codex.Update(Mapper.Map<Blizzard.Guild, Data.Guild>(blizzardGuild)));
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<Guild> RefreshNextGuild()
        {
            var guildToRefresh = _codex.GetGuilds().Where(g => g.LastRefreshError < 5).OrderBy(g => g.LastRefresh).First();
            return await RefreshGuild(guildToRefresh.Name, guildToRefresh.Realm);
        }

        public async Task<Guild> RefreshGuild(string name, string realm)
        {
            Blizzard.Guild blizzardGuild = await Task.Run(() => _blizzard.GetGuild(name, realm));
            if (blizzardGuild != null)
            {
                return await Task.Run(() => _codex.Update(Mapper.Map<Blizzard.Guild, Data.Guild>(blizzardGuild)));
            }
            return null;
        }

        public async Task RefreshCharactersAsync()
        {
            var guilds = await Task.Run(() => _codex.GetGuilds());
            var unguilded = await Task.Run(() => _codex.GetUnguildedMembers());
            foreach(Data.Guild guild in guilds)
            {
                foreach(Data.GuildMember member in guild.Members)
                {
                    Blizzard.Character blizzardCharacter = await Task.Run(() => _blizzard.GetCharacter(member.Name, member.Realm));
                    var character = Mapper.Map<Blizzard.Character, Data.Character>(blizzardCharacter);
                    await Task.Run(() => _codex.Save(character, true));
                }
            }

            foreach(GuildMember member in unguilded)
            {
                Blizzard.Character blizzardCharacter = await Task.Run(() => _blizzard.GetCharacter(member.Name, member.Realm));
                var character = Mapper.Map<Blizzard.Character, Data.Character>(blizzardCharacter);
                await Task.Run(() => _codex.Save(character, true));
            }
        }

        public async Task<Character> RefreshNextCharacter()
        {
            var characterToRefresh = await Task.Run(() => _codex.GetCharacters().Where(c => c.LastRefreshError < 5).OrderBy(c => c.LastRefresh).First());
            return await Task.Run(() => RefreshCharacter(characterToRefresh.Name, characterToRefresh.Realm));
        }

        public async Task<Character> InitializeNextCharacter()
        {
            var member = await Task.Run(() => _codex.GetUninitializedGuildMembers().FirstOrDefault());
            if (member != null)
            {
                var blizzardCharacter = await Task.Run(() => _blizzard.GetCharacter(member.Name, member.Realm));
                var character = Mapper.Map<Blizzard.Character, Data.Character>(blizzardCharacter);
                return await Task.Run(() => _codex.Create(character));
            }
            return null;
        }

        public async Task<Character> RefreshCharacter(string name, string realm)
        {
            Blizzard.Character blizzardCharacter = await Task.Run(() => _blizzard.GetCharacter(name, realm));
            var character = Mapper.Map<Blizzard.Character, Data.Character>(blizzardCharacter);
            return await Task.Run(() => _codex.Save(character, true));
        }

        public async Task<List<WoWClass>> GetClassesAsync()
        {
            return await Task.Run(() => _codex.GetClasses());
        }

        
    }
}
