namespace PrimeNumber.Client.RateLimiter
{
    public class FixedWindowRateLimiter : IRateLimiter
    {
        private readonly int _requestPerWindow;
        private readonly double _windowSizeSec;

        private int _counter = 0;
        private DateTime _lastWindowTime;

        public FixedWindowRateLimiter(int requestsPerSecond, double windowSizeSec = 1d)
        {
            _requestPerWindow = (int)(requestsPerSecond * windowSizeSec);
            _windowSizeSec = windowSizeSec;
        }

        public void Start()
        {
            _counter = 0;
            _lastWindowTime = DateTime.Now;
        }

        public async Task WaitForSlot(CancellationToken stoppingToken)
        {
            // Check if the number of requests made in the current window exceeds the limit
            // and if the current time is still within the same window.
            if (_counter == _requestPerWindow && (DateTime.Now - _lastWindowTime).TotalSeconds < _windowSizeSec)
            {
                // Calculate the remaining time in the current window and delay execution.
                var restTime = 1d - (DateTime.Now - _lastWindowTime).TotalSeconds;
                await Task.Delay(TimeSpan.FromSeconds(restTime), stoppingToken);

                // Move the last window time to the start of the next window.
                _lastWindowTime = _lastWindowTime.AddSeconds(1);
                _counter = 0;
            }
            else if ((DateTime.Now - _lastWindowTime).TotalSeconds >= _windowSizeSec)
            {
                // If the current time is beyond the current window, move to the next window.
                _lastWindowTime = _lastWindowTime.AddSeconds(1);
                _counter = 0;
            }

            _counter++;
        }
    }
}
