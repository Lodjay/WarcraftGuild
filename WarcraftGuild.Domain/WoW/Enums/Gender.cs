using WarcraftGuild.Domain.Core.Extensions;

namespace WarcraftGuild.Domain.WoW.Enums
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