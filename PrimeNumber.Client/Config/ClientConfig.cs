namespace PrimeNumber.Client.Config
{
    public class ClientConfig: IClientConfig
    {
        public string Url { get; set; }

        public int ExecutionTime { get; set; }

        public int RequestsPerSecond { get; set; }

        public long NumberLimit { get; set; }
    }
}
