using PowerArgs;

namespace PrimeNumber.Client.Config
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    public class ClientConfig: IClientConfig
    {
        [HelpHook, ArgShortcut("-h"), ArgDescription("Shows this help")]
        public bool Help { get; set; }

        [ArgShortcut("a")]
        [ArgRequired]
        [ArgDefaultValue("https://localhost:7091")]
        [ArgDescription("PrimeNumber Serice address")]
        public string RemoteAddress { get; set; }

        [ArgShortcut("t")]
        [ArgRequired]
        [ArgDefaultValue(1)]
        [ArgDescription("Execution time in seconds")]
        public int ExecutionTime { get; set; }

        [ArgShortcut("r")]
        [ArgRequired]
        [ArgDefaultValue(10000)]
        [ArgDescription("Requests count per second")]
        public int RequestsPerSecond { get; set; }

        [ArgShortcut("l")]
        [ArgRequired]
        [ArgDefaultValue(1000)]
        [ArgDescription("Max numer value")]
        public long NumberLimit { get; set; }
    }
}
