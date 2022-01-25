namespace WarcraftGuild.Domain.Interfaces
{
    public interface ILimiter
    {
        int RatesPerTimespan { get; set; }
        TimeSpan TimeBetweenLimitReset { get; set; }

        bool IsAtRateLimit();

        void OnHttpRequest();
    }
}