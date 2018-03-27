using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Primes.Lib
{
    public class PrimeCalc
    {
        ConcurrentBag<MinMaxPair> _primes = new ConcurrentBag<MinMaxPair>();

        private void CalcPrimesConcurrently(long min, long max)
        {
            var numbers = PrimeCalc.GetPrimes(min, max);
            _primes.Add(numbers);
        }

        public async Task<PrimeSummary> CalcPrimesForTime(long min, long max, long time)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            long targetMilliseconds = time * 1000;

            while (stopwatch.ElapsedMilliseconds < targetMilliseconds)
            {
                var t1 = Task.Run(() => CalcPrimesConcurrently(min, max));
                var t2 = Task.Run(() => CalcPrimesConcurrently(min, max));
                await Task.WhenAll(new Task[] { t1, t2 });
            }

            stopwatch.Stop();

            var summary = new PrimeSummary
            {
                TotalCalcs = _primes.Count,
                ElapsedSeconds = stopwatch.ElapsedMilliseconds / 1000
            };

            return summary;
        }


        public static MinMaxPair GetPrimes(long min, long max)
        {
            var primes = new MinMaxPair { Min = long.MaxValue, Max = long.MinValue };
            for (long x = min; x <= max; x++)
            {
                if (IsPrime(x))
                {
                    if (x < primes.Min)
                    {
                        primes.Min = x;
                    }
                    primes.Max = x;
                }
            }

            return primes;
        }

        public static bool IsPrime(long n)
        {
            if (n < 2) return false;
            if (n < 4) return true;
            if (n % 2 == 0) return false;

            double sqrt = System.Math.Sqrt(n);
            int sqrtCeiling = (int)System.Math.Ceiling(sqrt);

            for (int i = 3; i <= sqrtCeiling; i += 2)
                if (n % i == 0) return false;
            return true;
        }
    }
}
