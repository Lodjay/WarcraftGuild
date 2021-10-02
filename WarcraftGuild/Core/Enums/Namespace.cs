using WarcraftGuild.Core.Extensions;

namespace WarcraftGuild.Core.Enums
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