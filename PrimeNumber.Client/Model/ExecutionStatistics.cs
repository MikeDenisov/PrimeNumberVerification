namespace PrimeNumber.Client.Model
{
    public class ExecutionStatistics
    {
        public int TotalRequests { get; set; }
        public int FailedRequests { get; set; }
        public long AverageRTT { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public bool IsComplete { get; set; }
        public TimeSpan ExecutionTime => FinishTime - StartTime;
        public int AverateRequestsPerSec => TotalRequests / (int)Math.Ceiling(ExecutionTime.TotalSeconds);
    }
}
