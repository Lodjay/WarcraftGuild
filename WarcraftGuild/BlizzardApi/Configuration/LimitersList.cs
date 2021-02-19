using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi.Interfaces;

namespace WarcraftGuild.BlizzardApi.Configuration
{
    public class LimitersList
    {
        private List<IRateLimiter> rateLimiters;

        public LimitersList()
        {
            rateLimiters = new List<IRateLimiter>();
        }

        public void Add(IRateLimiter limiter)
        {
            if (rateLimiters == null)
                rateLimiters = new List<IRateLimiter>();

            rateLimiters.Add(limiter);
        }

        public void Remove(IRateLimiter limiter)
        {
            if (rateLimiters.Contains(limiter) == false)
                throw new KeyNotFoundException();

            rateLimiters.Remove(limiter);
        }

        public void Clear()
        {
            rateLimiters = new List<IRateLimiter>();
        }

        public bool AnyReachedLimit()
        {
            return rateLimiters.Any(i => i.IsAtRateLimit());
        }

        public void NotifyAll(BlizzardApiReader reader, IApiResponse responseMessage)
        {
            rateLimiters.ForEach(i => i.OnHttpRequest(reader, responseMessage));
        }
    }
}
