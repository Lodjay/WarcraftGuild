using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarcraftGuild.BlizzardApi.Interfaces
{
    public interface IRateLimiter
    {
        bool IsAtRateLimit();

        void OnHttpRequest(BlizzardApiReader reader, IApiResponse response);

    }
}
