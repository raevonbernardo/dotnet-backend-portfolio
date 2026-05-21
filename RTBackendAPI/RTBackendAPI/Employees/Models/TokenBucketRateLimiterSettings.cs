using System.Threading.RateLimiting;

namespace RTBackendAPI.Employees.Models;

public readonly struct TokenBucketRateLimiterSettings
{
    public readonly int TokenLimit;

    public readonly int TokenPerPeriod;

    public readonly TimeSpan ReplenishmentPeriod;

    public readonly int QueueLimit;

    public readonly bool AutoReplenishment;

    public TokenBucketRateLimiterSettings(int tokenLimit, int tokenPerPeriod, TimeSpan replenishmentPeriod, 
        int queueLimit, bool autoReplenishment)
    {
        this.TokenLimit = tokenLimit;
        this.TokenPerPeriod = tokenPerPeriod;
        this.ReplenishmentPeriod = replenishmentPeriod;
        this.QueueLimit = queueLimit;
        this.AutoReplenishment = autoReplenishment;
    }

    public TokenBucketRateLimiterSettings(int tokenLimit, int tokenPerPeriod, int replenishmentPeriodInSeconds, 
        int queueLimit, bool autoReplenishment)
    {
        this.TokenLimit = tokenLimit;
        this.TokenPerPeriod = tokenPerPeriod;
        this.ReplenishmentPeriod = TimeSpan.FromSeconds(replenishmentPeriodInSeconds);
        this.QueueLimit = queueLimit;
        this.AutoReplenishment = autoReplenishment;
    }

    public static TokenBucketRateLimiterSettings Default()
    {
        return new TokenBucketRateLimiterSettings(
            tokenLimit: 50,
            tokenPerPeriod: 10,
            replenishmentPeriod: TimeSpan.FromSeconds(15),
            queueLimit: 10,
            autoReplenishment: true);
    }
}