using WarcraftGuild.Domain.Core.Extensions;
using WarcraftGuild.Domain.Core.Json;
using WarcraftGuild.Domain.WoW.Enums;

namespace WarcraftGuild.Domain.WoW.Models
{
    public class Character : WoWModel
    {
        public string Name { get; set; }
        public Faction Faction { get; set; }
        public Gender Gender { get; set; }
        public Race Race { get; set; }
        public uint ClassID { get; set; }
        public ushort Level { get; set; }
        public List<AchievementCompletion> Achievements { get; set; }
        public List<AchievementCategoryCompletion> AchievementCategoryCompletion { get; set; }
        public Uri Avatar { get; set; }
        public Uri Inset { get; set; }
        public Uri Main { get; set; }
        public Uri MainRaw { get; set; }
        public ulong? MainId { get; set; }
        public Role MainRole { get; set; }
        public string Description { get; set; }

        public bool IsMain
        { get { return !MainId.HasValue; } }

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
                    Race = new Race { BlizzardId = characterJson.Race.Id };
                Level = characterJson.Level;
                if (characterJson.Media != null)
                {
                    if (characterJson.Media.Assets != null)
                    {
                        if (characterJson.Media.Assets.Any(x => x.Key == "avatar"))
                            Avatar = new Uri(characterJson.Media.Assets.Find(x => x.Key == "avatar").Value);
                        if (characterJson.Media.Assets.Any(x => x.Key == "inset"))
                            Inset = new Uri(characterJson.Media.Assets.Find(x => x.Key == "inset").Value);
                        if (characterJson.Media.Assets.Any(x => x.Key == "main"))
                            Main = new Uri(characterJson.Media.Assets.Find(x => x.Key == "main").Value);
                        if (characterJson.Media.Assets.Any(x => x.Key == "main-raw"))
                            MainRaw = new Uri(characterJson.Media.Assets.Find(x => x.Key == "main-raw").Value);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(characterJson.Media.AvatarUrl))
                            Avatar = new Uri(characterJson.Media.AvatarUrl);
                        if (!string.IsNullOrEmpty(characterJson.Media.InsetUrl))
                            Inset = new Uri(characterJson.Media.InsetUrl);
                        if (!string.IsNullOrEmpty(characterJson.Media.RenderUrl))
                            Main = new Uri(characterJson.Media.RenderUrl);
                    }
                }
            }
        }
    }
}