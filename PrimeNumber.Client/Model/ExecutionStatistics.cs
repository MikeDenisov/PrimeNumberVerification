namespace PrimeNumber.Client.Model
{
    public class ExecutionStatistics
    {
        public double RequestsPerSecond { get; set; }
        public long TotalRequests { get; set; }
        public long FailedRequests { get; set; }
        public long AverageRTT { get; set; }
    }
}
