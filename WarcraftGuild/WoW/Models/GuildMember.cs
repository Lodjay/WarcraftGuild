using WarcraftGuild.BlizzardApi.Json;

namespace WarcraftGuild.WoW.Models
{
    public class GuildMember : Character
    {
        public int Rank { get; set; }

        public GuildMember() : base()
        {
        }

        public GuildMember(GuildMemberJson guildMemberJson) : base()
        {
            Load(guildMemberJson);
        }

        private void Load(GuildMemberJson guildMemberJson)
        {
            if (CheckJson(guildMemberJson))
            {
                Rank = guildMemberJson.Rank;
                Load(guildMemberJson.Member);
            }
        }
    }
}