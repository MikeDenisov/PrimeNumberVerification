using System.Collections.Generic;

namespace PrimeNumber.Shared
{
    public class PrimeNumberValidator: IPrimeNumberValidator
    {
        private readonly HashSet<long> _primeNumbers;

        /// <summary>
        /// Create new validator
        /// </summary>
        /// <param name="limit">Max supported number</param>
        public PrimeNumberValidator(long limit)
        {
            Limit = limit;

            _primeNumbers = new HashSet<long>(GetPrimeNumbers(Limit));
        }

        /// <summary>
        /// Max number supported for validation
        /// </summary>
        public long Limit { get; }

        /// <summary>
        /// Validates number if it is prime
        /// </summary>
        public bool IsValid(long number)
        {
            return _primeNumbers.Contains(number);
        }

        /// <summary>
        /// The sieve of Atkin algorithm implementation
        /// https://www.geeksforgeeks.org/sieve-of-atkin/
        /// Time O(n / log(log(n)))
        /// Space O(n)
        /// </summary>
        /// <param name="limit">Max value</param>
        /// <returns>Enumeration of prime numbers up to the limit</returns>
        private IEnumerable<long> GetPrimeNumbers(long limit)
        {
            // no prime numbers exists before 2
            if (limit < 2)
                yield break;

            // 2 and 3 are known to be prime
            if (limit > 2)
                yield return 2;

            if (limit > 3)
                yield return 3;

            // Initialise the sieve array with
            // false values
            var sieve = new bool[limit + 1];

            for (var i = 0; i <= limit; i++)
                sieve[i] = false;

            /* Mark sieve[n] is true if one of the
            following is true:
            a) n = (4*x*x)+(y*y) has odd number 
               of solutions, i.e., there exist 
               odd number of distinct pairs 
               (x, y) that satisfy the equation 
               and    n % 12 = 1 or n % 12 = 5.
            b) n = (3*x*x)+(y*y) has odd number 
               of solutions and n % 12 = 7
            c) n = (3*x*x)-(y*y) has odd number 
               of solutions, x > y and n % 12 = 11 */
            for (var x = 1; x * x <= limit; x++)
            {
                for (var y = 1; y * y <= limit; y++)
                {

                    // Main part of Sieve of Atkin
                    var n = (4 * x * x) + (y * y);
                    if (n <= limit && (n % 12 == 1 || n % 12 == 5))
                        sieve[n] ^= true;

                    n = (3 * x * x) + (y * y);
                    if (n <= limit && n % 12 == 7)
                        sieve[n] ^= true;

                    n = (3 * x * x) - (y * y);
                    if (x > y && n <= limit && n % 12 == 11)
                        sieve[n] ^= true;
                }
            }

            // Mark all multiples of squares as
            // non-prime
            for (var r = 5; r * r < limit; r++)
            {
                if (sieve[r])
                {
                    for (var i = r * r; i < limit; i += r * r)
                        sieve[i] = false;
                }
            }

            // Return primes using sieve[]
            for (var a = 5; a <= limit; a++)
                if (sieve[a])
                    yield return a;

            yield break;
        }
    }
}
