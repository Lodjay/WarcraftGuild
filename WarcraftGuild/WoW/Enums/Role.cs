using WarcraftGuild.Core.Extensions;

namespace WarcraftGuild.WoW.Enums
{
    public enum Role
    {
        Unknown,

        [EnumCode("TANK")]
        Tank,

        [EnumCode("HEALER")]
        Healer,

        MeleeHealer,
        RangeHealer,

        [EnumCode("DAMAGE")]
        DamageDealer,

        MeleeDamageDealer,
        RangeDamageDealer,
    }
}