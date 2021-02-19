using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarcraftGuild.Exceptions
{
    public class RateLimitReachedException : Exception
    {
        public RateLimitReachedException(string message) : base(message)
        {
        }
    }

}
