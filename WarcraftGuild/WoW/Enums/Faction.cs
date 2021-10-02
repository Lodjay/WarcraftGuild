using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
