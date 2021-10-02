using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi.Json;
using WarcraftGuild.Core.Extensions;
using WarcraftGuild.WoW.Enums;

namespace WarcraftGuild.WoW.Models
{
    public class Character : WoWData
    {
        public ulong BlizzardId { get; protected set; }
        public string Name { get; protected set; }
        public Faction Faction { get; private set; }
        public Gender Gender { get; private set; }
        public uint RaceID { get; private set; }
        public uint ClassID { get; private set; }
        public ushort Level { get; private set; }
        public List<AchievementCompletion> Achievements { get; private set; }
        public List<AchievementCategoryCompletion> AchievementCategoryCompletion { get; private set; }


        public Character()
        {
            Achievements = new List<AchievementCompletion>();
            AchievementCategoryCompletion = new List<AchievementCategoryCompletion>();
        }

        public Character(CharacterJson characterJson) : this()
        {
            Load(characterJson);
        }

        public Character(MemberJson memberJson) : this(memberJson.Character)
        {
            Load(memberJson);
        }

        protected void Load(MemberJson memberJson)
        {
            if (CanLoadJson(memberJson))
            {
                BlizzardId = memberJson.Id;
            }
        }

        protected void Load(CharacterJson characterJson)
        {
            if (CanLoadJson(characterJson))
            {
                BlizzardId = characterJson.Id;
                Name = characterJson.Name;
                Faction = characterJson.Faction.Type.ParseCode<Faction>();
                Gender = characterJson.Gender.Type.ParseCode<Gender>();
                ClassID = characterJson.Class.Id;
                RaceID = characterJson.Race.Id;
                Level = characterJson.Level;
            }
        }
    }

    public class GuildMember : Character
    {
        public int Rank { get; private set; }

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
