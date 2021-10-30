using WarcraftGuild.BlizzardApi.Json;
using WarcraftGuild.Core.Extensions;
using WarcraftGuild.WoW.Enums;

namespace WarcraftGuild.WoW.Models
{
    public class Race : WoWModel
    {
        public string Name { get; set; }
        public string MaleName { get; set; }
        public string FemaleName { get; set; }
        public Faction Faction { get; set; }
        public bool Selectable { get; set; }
        public bool AlliedRace { get; set; }

        public Race()
        {
        }

        public Race(RaceJson raceJson) : this()
        {
            Load(raceJson);
        }

        public void Load(RaceJson raceJson)
        {
            if (CheckJson(raceJson))
            {
                BlizzardId = raceJson.Id;
                Name = raceJson.Name;
                if (raceJson.GenderNames != null)
                {
                    MaleName = raceJson.GenderNames.Male;
                    FemaleName = raceJson.GenderNames.Female;
                }
                if (raceJson.Faction != null)
                    Faction = raceJson.Faction.Type.ParseCode<Faction>();
                Selectable = raceJson.Selectable;
                AlliedRace = raceJson.AlliedRace;
            }
        }
    }
}