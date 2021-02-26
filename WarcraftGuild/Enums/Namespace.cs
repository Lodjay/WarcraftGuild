using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarcraftGuild.Enums
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
