using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarcraftGuild.Core.Extensions;

namespace WarcraftGuild.Core.Enums
{
    public enum NamedHttpClients
    {
        [EnumCode("ApiClient")]
        ApiClient,
        [EnumCode("AuthClient")]
        AuthClient
    }
}
