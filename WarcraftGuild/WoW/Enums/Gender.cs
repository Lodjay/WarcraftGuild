using WarcraftGuild.Core.Extensions;

namespace WarcraftGuild.WoW.Enums
{
    public enum Gender
    {
        Unknown,

        [EnumCode("MALE")]
        Male,

        [EnumCode("FEMALE")]
        Female,
    }
}