namespace Rate_limiting_Example
{
    public class MyRateLimitOptions
    {
        public const string MyRateLimit = "MyRateLimit";
        public int PermitLimit { get; set; } = 100;
        public int Window { get; set; } = 10;
        public int ReplenishmentPeriod { get; set; } = 2;
        public int QueueLimit { get; set; } = 2;
        public int SegmentsPerWindow { get; set; } = 8;
        public int TokenLimit { get; set; } = 10;
        public int TokensPerPeriod { get; set; } = 4;
        public bool AutoReplenishment { get; set; } = false;
    }
}
