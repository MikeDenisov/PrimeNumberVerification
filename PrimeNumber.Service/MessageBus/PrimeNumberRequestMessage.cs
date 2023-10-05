namespace PrimeNumber.Service.MessageBus
{
    public struct PrimeNumberRequestMessage
    {
        public long Number { get; set; }
        public bool IsValid { get; set; }
    }
}
