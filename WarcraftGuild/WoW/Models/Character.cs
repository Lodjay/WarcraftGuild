using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi.WoWJson;
using WarcraftGuild.Core.Extensions;
using WarcraftGuild.WoW.Enums;

namespace WarcraftGuild.WoW.Models
{
    public class Character
    {
        public ulong BlizzardId { get; private set; }
        public string Name { get; private set; }
        public Faction Faction { get; private set; }
        public Gender Gender { get; private set; }
        public int RaceID { get; private set; }
        public int ClassID { get; private set; }
        public int Level { get; private set; }
        public List<AchievementCompletion> Achievements { get; private set; }
        public List<AchievementCategoryCompletion> AchievementCategoryCompletion { get; private set; }


        public Character()
        {
            Achievements = new List<AchievementCompletion>();
            AchievementCategoryCompletion = new List<AchievementCategoryCompletion>();
        }

        public Character(CharacterJson characterJson) : this()
        {
            BlizzardId = characterJson.Id;
            Name = characterJson.Name;
            Faction = characterJson.Faction.Type.ParseCode<Faction>();
            Gender = characterJson.Gender.Type.ParseCode<Gender>();
            ClassID = characterJson.Class.Id;
            RaceID = characterJson.Race.Id;
            Level = characterJson.Level;
        }

        public Character(MemberJson memberJson) : this()
        {
            BlizzardId = memberJson.Id;
        }
    }

    public class GuildMember : Character
    {
        public int Rank { get; private set; }

        public GuildMember() : base()
        {

        }

        public GuildMember(GuildMemberJson guildMemberJson) : base(guildMemberJson.Member)
        {
            Rank = guildMemberJson.Rank;
        }
    }
}
