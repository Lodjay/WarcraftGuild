using WarcraftGuild.Domain.Core.Extensions;

namespace WarcraftGuild.Domain.WoW.Enums
{
    public enum Faction
    {
        Unknown,

        [EnumCode("ALLIANCE")]
        Alliance,

        [EnumCode("HORDE")]
        Horde,
    }
}