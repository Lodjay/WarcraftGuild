using WarcraftGuild.Domain.Core.Extensions;

namespace WarcraftGuild.Domain.Core.Enums
{
    public enum Region
    {
        [EnumCode("EU")]
        Europe,

        [EnumCode("KR")]
        Korea,

        [EnumCode("SEA")] //TODO: is this a valid Region?
        SoutheastAsia,

        [EnumCode("TW")]
        Taiwan,

        [EnumCode("US")]
        UnitedStates,

        [EnumCode("CN")]
        China,
    }
}