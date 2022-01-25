using WarcraftGuild.Domain.Interfaces;

namespace WarcraftGuild.Infrastructure.BlizzardApi.Configuration
{
    public class Limiter : ILimiter
    {
        public int RatesPerTimespan { get; set; }
        public TimeSpan TimeBetweenLimitReset { get; set; }

        private DateTime timeUntilTheNextReset;
        private int currentRateCounter = 0;

        public Limiter()
        {
            currentRateCounter = 0;
            CalculateAndSetNextReset();
        }

        public Limiter(TimeSpan timeBetweenReset, int maxRatePerReset)
        {
            TimeBetweenLimitReset = timeBetweenReset;
            RatesPerTimespan = maxRatePerReset;

            currentRateCounter = 0;
            CalculateAndSetNextReset();
        }

        public bool IsAtRateLimit()
        {
            if (IsReadyToReset())
                ResetLimiter();

            return currentRateCounter >= RatesPerTimespan;
        }

        public void OnHttpRequest()
        {
            if (IsReadyToReset())
                ResetLimiter();

            currentRateCounter++;
        }

        private void CalculateAndSetNextReset()
        {
            timeUntilTheNextReset = DateTime.Now.Add(TimeBetweenLimitReset);
        }

        private bool IsReadyToReset()
        {
            return DateTime.Now >= timeUntilTheNextReset;
        }

        private void ResetLimiter()
        {
            currentRateCounter = 0;
            CalculateAndSetNextReset();
        }
    }
}