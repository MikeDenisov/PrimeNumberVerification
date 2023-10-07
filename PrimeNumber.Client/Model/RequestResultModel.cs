using Grpc.Core;

namespace PrimeNumber.Client.Model
{
    public struct RequestResultModel
    {
        public long Id { get; set; }
        public long Number { get; set; }
        public bool RemoteResult { get; set; }
        public TimeSpan RoundTripTime { get; set; }
        public Status Status { get; set; }
    }
}
