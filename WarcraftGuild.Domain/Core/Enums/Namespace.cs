using WarcraftGuild.Domain.Core.Extensions;

namespace WarcraftGuild.Domain.Core.Enums
{
    public enum Namespace
    {
        [EnumCode("static")]
        Static,

        [EnumCode("dynamic")]
        Dynamic,

        [EnumCode("profile")]
        Profile,
    }
}