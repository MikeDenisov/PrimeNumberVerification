namespace PrimeNumber.Shared
{
    public interface IPrimeNumberValidator
    {
        /// <summary>
        /// Max number supported for validation
        /// </summary>
        public long Limit { get; }

        /// <summary>
        /// Validates number if it is prime
        /// </summary>
        public bool IsValid(long number);
    }
}
