namespace PrimeNumber.Client.Model
{
    public struct RequestResultModel
    {
        public long Id { get; set; }
        public long Number { get; set; }
        public bool RemoteResult { get; set; }
        public bool IsSuccessfull { get; set; }
        public TimeSpan RoundTripTime { get; set; }
    }
}
