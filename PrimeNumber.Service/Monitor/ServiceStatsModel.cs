namespace PrimeNumber.Service.Monitor
{
    public class ServiceStatsModel
    {
        public DateTime TimeStamp { get; set; }
        public IDictionary<long,long> TopRequestedNumbers { get; set; }
        public double RequestsPerSecond { get; set; }
        public long RequestsTotalCount { get; set; }
    }
}
