using WarcraftGuild.Domain.Core.Extensions;
using WarcraftGuild.Domain.Core.Json;
using WarcraftGuild.Domain.WoW.Enums;

namespace WarcraftGuild.Domain.WoW.Models
{
    public class Specialization : WoWModel
    {
        public string Name { get; set; }
        public ulong ClassId { get; set; }
        public string MaleDescription { get; set; }
        public string FemaleDescription { get; set; }
        public Role Role { get; set; }
        public Uri Icon { get; set; }

        public Specialization()
        {
        }

        public Specialization(SpecializationJson specializationJson) : this()
        {
            Load(specializationJson);
        }

        public void Load(SpecializationJson specializationJson)
        {
            if (CheckJson(specializationJson))
            {
                BlizzardId = specializationJson.Id;
                Name = specializationJson.Name;
                if (specializationJson.Class != null)
                    ClassId = specializationJson.Class.Id;
                if (specializationJson.GenderDescriptions != null)
                {
                    MaleDescription = specializationJson.GenderDescriptions.Male;
                    FemaleDescription = specializationJson.GenderDescriptions.Female;
                }
                if (specializationJson.Role != null)
                    Role = specializationJson.Role.Type.ParseCode<Role>();
                if (specializationJson.Media != null && specializationJson.Media.Assets != null && specializationJson.Media.Assets.Any(x => x.Key == "icon"))
                    Icon = new Uri(specializationJson.Media.Assets.Find(x => x.Key == "icon").Value);
            }
        }
    }
}