using WarcraftGuild.Core.Extensions;

namespace WarcraftGuild.WoW.Enums
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