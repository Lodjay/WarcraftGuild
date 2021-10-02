using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarcraftGuild.Core.Extensions;

namespace WarcraftGuild.Core.Enums
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
