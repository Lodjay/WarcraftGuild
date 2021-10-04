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
            if (CanLoadJson(guildMemberJson))
            {
                Rank = guildMemberJson.Rank;
                if (CanLoadJson(guildMemberJson.Member))
                {
                    if (CanLoadJson(guildMemberJson.Member.Character))
                        Load(guildMemberJson.Member.Character);
                    else
                    {
                        BlizzardId = guildMemberJson.Member.Id;
                        Name = guildMemberJson.Member.Name;
                    }
                }
            }
        }
    }
}
