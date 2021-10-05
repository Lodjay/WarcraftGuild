using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi.Json;
using WarcraftGuild.Core.Extensions;
using WarcraftGuild.WoW.Enums;

namespace WarcraftGuild.WoW.Models
{
    public class Character : WoWModel
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

        protected void Load(CharacterJson characterJson)
        {
            if (CheckJson(characterJson))
            {
                BlizzardId = characterJson.Id;
                Name = characterJson.Name;
                if (characterJson.Faction != null)
                    Faction = characterJson.Faction.Type.ParseCode<Faction>();
                if (characterJson.Gender != null)
                    Gender = characterJson.Gender.Type.ParseCode<Gender>();
                if (characterJson.Class != null)
                    ClassID = characterJson.Class.Id;
                if (characterJson.Race != null)
                    RaceID = characterJson.Race.Id;
                Level = characterJson.Level;
            }
        }
    }
}
