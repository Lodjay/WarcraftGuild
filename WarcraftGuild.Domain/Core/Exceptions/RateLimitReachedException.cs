namespace WarcraftGuild.Domain.Core.Exceptions
{
    public class RateLimitReachedException : Exception
    {
        public RateLimitReachedException(string message) : base(message)
        {
        }
    }
}