using WarcraftGuild.Domain.Core.Json;

namespace WarcraftGuild.Domain.WoW.Models
{
    public class Realm : WoWModel
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Category { get; set; }
        public string Locale { get; set; }
        public string Timezone { get; set; }
        public string Type { get; set; }
        public bool Tournament { get; set; }

        public Realm()
        {
        }

        public Realm(RealmJson realmJson) : this()
        {
            Load(realmJson);
        }

        public void Load(RealmJson realmJson)
        {
            if (CheckJson(realmJson))
            {
                BlizzardId = realmJson.Id;
                Name = realmJson.Name;
                Slug = realmJson.Slug;
                Category = realmJson.Category;
                Locale = realmJson.Locale;
                Timezone = realmJson.Timezone;
                Tournament = realmJson.Tournament;
                Type = realmJson.Type?.Name;
            }
        }
    }
}