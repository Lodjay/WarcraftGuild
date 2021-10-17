using WarcraftGuild.BlizzardApi.Json;

namespace WarcraftGuild.WoW.Models
{
    public class GuildMember
    {
        public int Rank { get; set; }
        public string Comment { get; set; }
        public ulong CharacterId { get; set; }
        public string CharacterName { get; set; }
        public string CharacterRealmSlug { get; set; }

        public GuildMember() : base()
        {
        }

        public GuildMember(GuildMemberJson guildMemberJson) : base()
        {
            Load(guildMemberJson);
        }

        private void Load(GuildMemberJson guildMemberJson)
        {
            if (guildMemberJson != null)
            {
                Rank = guildMemberJson.Rank;
                if (guildMemberJson.Member != null)
                {
                    CharacterId = guildMemberJson.Member.Id;
                    CharacterName = guildMemberJson.Member.Name;
                    if (guildMemberJson.Member.Realm != null)
                        CharacterRealmSlug = guildMemberJson.Member.Realm.Slug;
                }
            }
        }
    }
}