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
        public string Name { get; set; }
        public Faction Faction { get; set; }
        public Gender Gender { get; set; }
        public uint RaceID { get; set; }
        public uint ClassID { get; set; }
        public ushort Level { get; set; }
        public List<AchievementCompletion> Achievements { get; set; }
        public List<AchievementCategoryCompletion> AchievementCategoryCompletion { get; set; }


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
}
