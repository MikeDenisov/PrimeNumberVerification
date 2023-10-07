namespace PrimeNumber.Service.Config
{
    public class ServiceConfig
    {
        public ServiceConfig(long primeNumberLimit, double outputDelaySec)
        {
            PrimeNumberLimit = primeNumberLimit;
            OutputDelaySec = outputDelaySec;
        }

        public long PrimeNumberLimit { get; }
        public double OutputDelaySec { get; }
    }
}