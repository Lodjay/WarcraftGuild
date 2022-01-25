using WarcraftGuild.Domain.Core.Extensions;

namespace WarcraftGuild.Domain.WoW.Enums
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