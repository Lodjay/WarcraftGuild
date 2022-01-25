using WarcraftGuild.Domain.Core.Json;

namespace WarcraftGuild.Domain.WoW.Models
{
    public class ConnectedRealm : WoWModel
    {
        public bool HasQueue { get; set; }
        public string Statut { get; set; }
        public string Population { get; set; }
        public List<string> RealmSlugs { get; set; }

        public ConnectedRealm()
        {
            RealmSlugs = new List<string>();
        }

        public ConnectedRealm(ConnectedRealmJson conntectedRealmJson) : this()
        {
            Load(conntectedRealmJson);
        }

        public void Load(ConnectedRealmJson conntectedRealmJson)
        {
            if (CheckJson(conntectedRealmJson))
            {
                BlizzardId = conntectedRealmJson.Id;
                HasQueue = conntectedRealmJson.HasQueue;
                Statut = conntectedRealmJson.Status?.Name;
                Population = conntectedRealmJson.Population?.Name;
                if (conntectedRealmJson.Realms != null)
                    foreach (RealmJson realmJson in conntectedRealmJson.Realms)
                        RealmSlugs.Add(realmJson.Slug);
            }
        }
    }
}